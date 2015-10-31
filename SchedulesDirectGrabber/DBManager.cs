using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
// TODO: Parameterize all queries!  http://stackoverflow.com/questions/2662999/system-data-sqlite-parameterized-queries-with-multiple-values

namespace SchedulesDirectGrabber
{
    using SDProgramImageResponse = ImageCache.SDProgramImageResponse;
    using SDProgram = ProgramCache.SDProgram;

    using DBSeriesInfo = SeriesInfoCache.DBSeriesInfo;
    using DBProgram = ProgramCache.DBProgram;
    using SDStationScheduleResponse = ScheduleCache.SDStationScheduleResponse;

    public class DBManager
    {
        public static DBManager instance = new DBManager();

        private DBManager() {
            OpenDB();
            CreateTables();
        }

        private void OpenDB() {
            connection_ = new SQLiteConnection("DataSource=GuideDataCache.db");
            connection_.Open();
            CreateTables();
        }

        private const string kProgramsTableName = "programs";
        private const string kSchedulesTableName = "schedules";
        private const string kImagesTableName = "images";
        private const string kProgramIdColumnName = "programID";
        private const string kProgramRawResponseColumnName = "sdResponse";
        private const string kProgramDataColumnName = "data";
        private const string kImageIdColumnName = "programID";
        private const string kImageDataColumnName = "JSON";
        private const string kMD5ColumnName = "md5";

        private const string kStationIDColumnName = "scheduleID";
        private const string kDayColumnName = "day";
        private const string kScheduleEntriesColumnName = "JSON";

        private const string kSeriesInfoTableName = "seriesinfo";
        private const string kSeriesIdColumnName = "id";
        private const string kSeriesDataColumnName = "JSON";

        private SQLiteCommand CreateSQLCommand(string sql)
        {
            return new SQLiteCommand(sql, connection_);
        }

        // returns # of rows affected
        private int ExecuteSQLCommand(string sql)
        {
            try {
                using (SQLiteCommand command = CreateSQLCommand(sql))
                    return command.ExecuteNonQuery();
            } catch (Exception e)
            {
                Misc.OutputException(e);
                throw;
            }
        }

        private int ExecuteSQLCommand(string sql, IDictionary<string, string> parameters)
        {
            using (SQLiteCommand command = CreateSQLCommand(sql))
            {
                foreach (var keyval in parameters)
                {
                    command.Parameters.Add(keyval.Key, System.Data.DbType.String).Value = keyval.Value;
                }
                return command.ExecuteNonQuery();
            }
        }

        private void CreateTables()
        {
            try
            {
                ExecuteSQLCommand(string.Format(
                    @"CREATE TABLE IF NOT EXISTS [{0}]
                    ([{1}] VARCHAR(15) NOT NULL PRIMARY KEY,
                    [{2}] TEXT NULL,
                    [{3}] TEXT NULL,
                    [{4}] VARCHAR(30) NULL);",
                    kProgramsTableName, kProgramIdColumnName, kProgramDataColumnName, kProgramRawResponseColumnName ,kMD5ColumnName));
                ExecuteSQLCommand(string.Format(
                    @"CREATE TABLE IF NOT EXISTS [{0}]
                    ([{1}] VARCHAR(15) NOT NULL,
                    [{2}] VARCHAR(11) NOT NULL,
                    [{3}] TEXT NOT NULL,
                    [{4}] VARCHAR(30) NOT NULL,
                    PRIMARY KEY ({1}, {2}));",
                    kSchedulesTableName, kStationIDColumnName, kDayColumnName, kScheduleEntriesColumnName, kMD5ColumnName));
                ExecuteSQLCommand(string.Format(
                    @"CREATE TABLE IF NOT EXISTS[{0}]
                    ([{1}] VARCHAR (11) NOT NULL PRIMARY KEY,
                    [{2}] TEXT NOT NULL);",
                    kImagesTableName, kImageIdColumnName, kImageDataColumnName));
                ExecuteSQLCommand(string.Format(
                    @"CREATE TABLE IF NOT EXISTS[{0}]
                    ([{1}] VARCHAR (20) NOT NULL PRIMARY KEY,
                    [{2}] TEXT NOT NULL);",
                    kSeriesInfoTableName, kSeriesIdColumnName, kSeriesDataColumnName));
            }
            catch (Exception e) {
                Misc.OutputException(e);
            }
        }

        private SQLiteConnection connection_ = null;

        public void SaveRawSDProgramResponses(IEnumerable<ProgramCache.SDProgram> programs)
        {
            Console.WriteLine("Saving Program JSON Responses to local DB");
            const string kProgramIDParam = "@programID";
            const string kProgramResponseParam = "@programResponse";
            string sql = string.Format("insert or replace into {0} ({1}, {2}) values ({3}, {4})",
                        kProgramsTableName, kProgramIdColumnName, kProgramRawResponseColumnName,
                                            kProgramIDParam, kProgramResponseParam);
            using (var transaction = connection_.BeginTransaction())
            {
                foreach (var program in programs)
                {
                    if (program.code > 0) continue;
                    Dictionary<string, string> paramDictionary = new Dictionary<string, string>();
                    int rowCount = ExecuteSQLCommand(sql, new Dictionary<string, string>()
                                            {
                                                {kProgramIDParam, program.programID},
                                                {kProgramResponseParam, JSONClient.Serialize(program)},
                                            });
                    if (rowCount != 1) throw new Exception("Expected exactly 1 row changed, got " + rowCount);
                }
                transaction.Commit();
            }
        }

        public void SaveProgramData(IEnumerable<DBProgram> programs)
        {
            Console.WriteLine("Saving program data to local DB");
            const string kProgramIDParam = "@programID";
            const string kProgramDataParam = "@programData";
            const string kMD5Param = "@md5";
            string sql = string.Format("insert or replace into {0} ({1}, {2}, {3}) values ({4}, {5}, {6})",
                        kProgramsTableName, kProgramIdColumnName, kProgramDataColumnName, kMD5ColumnName,
                                            kProgramIDParam, kProgramDataParam, kMD5Param);
            using (var transaction = connection_.BeginTransaction())
            {
                foreach (var program in programs)
                {
                    Dictionary<string, string> paramDictionary = new Dictionary<string, string>();
                    int rowCount = ExecuteSQLCommand(sql, new Dictionary<string, string>()
                                            {
                                                {kProgramIDParam, program.programID},
                                                {kProgramDataParam, JSONClient.Serialize(program)},
                                                {kMD5Param, program.md5}
                                           });
                    if (rowCount != 1) throw new Exception("Expected exactly 1 row changed, got " + rowCount);
                }
                transaction.Commit();
            }
        }

        public IDictionary<string, DBProgram> GetProgramsByIds(ISet<string> programIDs)
        {
            Console.WriteLine("Reading program info from local DB");
            Dictionary<string, DBProgram> programsByID = new Dictionary<string, DBProgram>();
            using (SQLiteCommand command = new SQLiteCommand(
                string.Format("select {1}, {2} from {0};", kProgramsTableName, kProgramIdColumnName, kProgramDataColumnName),
                connection_))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string programID = reader[0].ToString();
                    string json = reader[1].ToString();
                    if (programIDs.Contains(programID) && !string.IsNullOrEmpty(json))
                        programsByID[programID] = JSONClient.Deserialize<DBProgram>(json);
                }
            }
            return programsByID;
        }

        internal void SaveSeriesInfos(IEnumerable<DBSeriesInfo> seriesInfos)
        {
            Console.WriteLine("Saving series info to local DB");
            const string kSeriesIdParam = "@seriesID";
            const string kSeriesDataParam = "@seriesData";
            string sql = string.Format("insert or replace into {0} ({1}, {2}) values ({3}, {4})",
                kSeriesInfoTableName, kSeriesIdColumnName, kSeriesDataColumnName,
                                      kSeriesIdParam,      kSeriesDataParam);
            using (var transsaction = connection_.BeginTransaction())
            {
                foreach(var seriesInfo in seriesInfos)
                {
                    int rowCount = ExecuteSQLCommand(sql, new Dictionary<string, string>()
                        {
                            {kSeriesIdParam, seriesInfo.id },
                            {kSeriesDataParam, JSONClient.Serialize(seriesInfo) }
                        });
                }
                transsaction.Commit();
            }
        }

        internal IDictionary<string, DBSeriesInfo> GetSeriesInfoByIDs(ISet<string> seriesIds)
        {
            Console.WriteLine("Reading series info from local DB");
            Dictionary<string, DBSeriesInfo> seriesById = new Dictionary<string, DBSeriesInfo>();
            using (SQLiteCommand command = new SQLiteCommand(
                string.Format("select {1}, {2} from {0};", kSeriesInfoTableName, kSeriesIdColumnName, kSeriesDataColumnName), connection_))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string seriesId = reader[0].ToString();
                    if (seriesIds.Contains(seriesId))
                        seriesById[seriesId] = JSONClient.Deserialize<DBSeriesInfo>(reader[1].ToString());
                }
            }
            return seriesById;
        }


        internal void SaveSchedules(IEnumerable<SDStationScheduleResponse> stationSchedules)
        {
            Console.WriteLine("Saving schedules to local DB");
            const string kStationIDParam = "@stationID";
            const string kDayParam = "@day";
            const string kScheduleEntriesParam = "@scheduleEntries";
            const string kMD5Param = "@md5";
            string sql = string.Format("insert or replace into {0} ({1}, {2}, {3}, {4}) values ({5}, {6}, {7}, {8});",
                        kSchedulesTableName, kStationIDColumnName, kDayColumnName, kScheduleEntriesColumnName, kMD5ColumnName,
                                             kStationIDParam, kDayParam, kScheduleEntriesParam, kMD5Param);
            using (var transaction = connection_.BeginTransaction())
            {
                foreach (var stationSchedule in stationSchedules)
                {
                    if (stationSchedule.code > 0) continue;
                    int rowCount = ExecuteSQLCommand(sql,
                        new Dictionary<string, string>() {
                            {kStationIDParam, stationSchedule.stationID },
                            {kDayParam, stationSchedule.metadata.startDate },
                            {kScheduleEntriesParam, JSONClient.Serialize(stationSchedule) },
                            {kMD5Param, stationSchedule.metadata.md5 } });
                    if (rowCount != 1) throw new Exception("Expected exactly 1 row changed, got " + rowCount);
                }
                transaction.Commit();
            }
            PruneSchedules();
        }

        // Note: image IDs should be used here, not program IDs.
        public IDictionary<string, SDProgramImageResponse> GetImagesByIds(ISet<string> imageIDs)
        {
            Console.WriteLine("Reading program images from DB");
            Dictionary<string, SDProgramImageResponse> imagesByID =
                new Dictionary<string, SDProgramImageResponse>();
            foreach (string id in imageIDs) imageIDs.Add(Misc.LimitString(id, 10));
            using (SQLiteCommand command = new SQLiteCommand(string.Format("select {1}, {2} from {0};",
                kImagesTableName, kImageIdColumnName, kImageDataColumnName), connection_))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader[0].ToString();
                    if (imageIDs.Contains(id))
                        imagesByID[id] = JSONClient.Deserialize<SDProgramImageResponse>(reader[1].ToString());
                }
            }
            return imagesByID;
        }

        public void SaveProgramImages(IEnumerable<SDProgramImageResponse> programImages)
        {
            Console.WriteLine("Saving program images to local DB");
            const string kImageIdParam = "@programID";
            const string kImageResponseParam = "@json";
            string sql = string.Format("insert or replace into {0} ({1}, {2}) values ({3}, {4});",
                        kImagesTableName, kImageIdColumnName, kImageDataColumnName,
                                          kImageIdParam, kImageResponseParam);
            using (var transaction = connection_.BeginTransaction())
            {
                foreach(var programImage in programImages)
                {
                    if (programImage.code > 0) continue;
                    int rowCount = ExecuteSQLCommand(sql, new Dictionary<string, string>() {
                            {kImageIdParam,       programImage.programID },
                            {kImageResponseParam, JSONClient.Serialize(programImage) } });
                    if (rowCount != 1) throw new Exception("Expected exactly 1 row changed, got " + rowCount);
                }
                transaction.Commit();
            }
        }

        private void PruneSchedules()
        {
            Console.WriteLine("Removing outdated schedules from local DB");
            const string kDayParam = "@day";
            ExecuteSQLCommand(string.Format("delete from {0} where {1} < {2};",
                kSchedulesTableName, kDayColumnName, kDayParam), new Dictionary<string, string>() {
                    {kDayParam, DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToUniversalTime().ToString("yyyy-MM-dd") } });
        }
    
        internal IDictionary<string, IDictionary<string, SDStationScheduleResponse>> GetStationSchedules(ISet<string> stationIDs)
        {
            Console.WriteLine("Reading schedules from local DB");
            Dictionary<string, IDictionary<string, SDStationScheduleResponse>> stationSchedules =
                new Dictionary<string, IDictionary<string, SDStationScheduleResponse>>();
            foreach (string stationID in stationIDs)
                stationSchedules[stationID] = new Dictionary<string, SDStationScheduleResponse>();
            using (SQLiteCommand command = new SQLiteCommand(
                string.Format("select {1}, {2} from {0};", kSchedulesTableName, kStationIDColumnName, kScheduleEntriesColumnName),
                connection_))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string stationID = reader[0].ToString();
                    if (stationIDs.Contains(stationID))
                    {
                        SDStationScheduleResponse dailyScheduleResponse =
                            JSONClient.Deserialize<SDStationScheduleResponse>(reader[1].ToString());
                        stationSchedules[stationID][dailyScheduleResponse.metadata.startDate] = dailyScheduleResponse;
                    }
                }
            }

            return stationSchedules;
        }
    }
}

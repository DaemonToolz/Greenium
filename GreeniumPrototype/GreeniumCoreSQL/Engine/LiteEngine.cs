﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumCoreSQL.Engine
{
    public class LiteEngine
    {
        private static readonly String DbName = "GreeniumBrain.sqlite";
        private static SQLiteConnection MyConnection { get; set; }

        public LiteEngine(){
            if (MyConnection == null)
            {
                if (!File.Exists(DbName))
                    Init();
                else
                {
                    MyConnection = new SQLiteConnection($"Data Source={DbName};Version=3;");
                    MyConnection.OpenAsync();
                }
            }
        }

        public void Init(){
            SQLiteConnection.CreateFile(DbName);
            MyConnection = new SQLiteConnection($"Data Source={DbName};Version=3;");
            MyConnection.Open();

            var CreateCmdStr = "CREATE TABLE History( url varchar(1024) not null, datebig integer not null, datesmall integer not null  )";
            using (var Command = new SQLiteCommand(CreateCmdStr, MyConnection))
            {
                Command.ExecuteNonQuery();
            }
        }

        public void Add(String url)
        {
            try
            {
                using (var Command = new SQLiteCommand($"INSERT INTO History(url, datebig, datesmall) values(@url,@dbig, @dsmall)",
                    MyConnection))
                {
                    Command.Parameters.AddWithValue("@url",$"'{url}'");
                    Command.Parameters.AddWithValue("@dbig", Int32.Parse(DateTime.Now.ToString("yyMMdd")));
                    Command.Parameters.AddWithValue("@dsmall", Int32.Parse(DateTime.Now.ToString("HHmmss")));
                    Command.ExecuteNonQuery();
                }

            }
            catch {
                throw;
            }
        }

        public void Remove(HistoryCommand specific)
        {
            try
            {
                using (var Command = new SQLiteCommand($"DELETE FROM History WHERE url=@url AND datebig=@dbig AND datesmall=@dsmall",
                    MyConnection))
                {

                    Command.Parameters.AddWithValue("@url", $"'{specific.URL}'");
                    Command.Parameters.AddWithValue("@dbig", Int32.Parse(specific.Time.ToString("yyMMdd")));
                    Command.Parameters.AddWithValue("@dsmall", Int32.Parse(specific.Time.ToString("HHmmss")));
                    Command.ExecuteNonQuery();


                }

            }
            catch  {
                throw;
            }
        }

        public IEnumerable<HistoryCommand> Read(String cls)
        {
            using (var Command = new SQLiteCommand($"SELECT * FROM {cls}", MyConnection)) {
                using (var reader = Command.ExecuteReader()) {
                    while (reader.Read())
                    {
                        var thisBigDate = reader.GetInt32(1).ToString().SplitInParts(2).ToList();
                        var thisSmallDate = reader.GetInt32(2).ToString().SplitInParts(2).ToList();
                        yield return new HistoryCommand(){
                            URL = reader.GetString(0),
                            Time = new DateTime(
                                int.Parse("20"+thisBigDate[0]),
                                int.Parse(thisBigDate[1]), int.Parse(thisBigDate[2]),
                                int.Parse(thisSmallDate[0]), int.Parse(thisSmallDate[1]), int.Parse(thisSmallDate[2])
                                )
                        };
                    }
                }
            }
        }

        public class HistoryCommand
        {
            public String URL { get; set; }
            public DateTime Time { get; set; }
        
        }
    }
}

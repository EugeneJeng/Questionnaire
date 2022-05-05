using Questionnaire.Helper;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Questionnaire.Managers
{
    public class AnswerManager
    {
        public List<AnswerModel> GetAnswerList(Guid queID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Answers
                    WHERE QueID = @QueID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QueID", queID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<AnswerModel> list = new List<AnswerModel>();
                        while (reader.Read())
                        {
                            AnswerModel model = BuildAnswerModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AnswerManager.GetAnswerList", ex);
                throw;
            }
        }
        public AnswerModel GetAnswer(Guid ansID, Guid userID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Answers
                    WHERE AnswerID = @ansID AND UserID = @userID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ansID", ansID);
                        command.Parameters.AddWithValue("@userID", userID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        AnswerModel model = new AnswerModel();
                        if (reader.Read())
                        {
                            model = BuildAnswerModel(reader);
                        }
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AnswerManager.GetAnswer", ex);
                throw;
            }
        }
        public List<AnswerModel> GetAnswerList(Guid ansID, Guid userID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Answers
                    WHERE AnswerID = @ansID AND UserID = @userID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@ansID", ansID);
                        command.Parameters.AddWithValue("@userID", userID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<AnswerModel> list = new List<AnswerModel>();
                        while (reader.Read())
                        {
                            AnswerModel model = BuildAnswerModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AnswerManager.GetAnswer", ex);
                throw;
            }
        }
        public void CreateAnswer(AnswerModel ans)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    INSERT INTO [Answers] (AnswerID, QueID, UserID, Answer) 
                    VALUES (@AnswerID, @QueID, @UserID, @Answer)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@AnswerID", ans.AnswerID);
                        command.Parameters.AddWithValue("@QueID", ans.QueID);
                        command.Parameters.AddWithValue("@UserID", ans.UserID);
                        command.Parameters.AddWithValue("@Answer", ans.Answer);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AnswerManager.CreateAnswer", ex);
                throw;
            }
        }
        public AnswerModel BuildAnswerModel(SqlDataReader reader)
        {
            return new AnswerModel()
            {
                AnswerID = (Guid)reader["AnswerID"],
                QueID = (Guid)reader["QueID"],
                UserID = (Guid)reader["UserID"],
                Answer = reader["Answer"] as string
            };
        }
    }
}
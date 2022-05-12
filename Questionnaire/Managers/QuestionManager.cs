using Questionnaire.Models;
using Questionnaire.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Questionnaire.Managers
{
    public class QuestionManager
    {
        public QuestionModel GetQuestionByID(Guid queID, Guid questionID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Questions
                    WHERE QueID = @queID AND QuestionID = @questionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@queID", queID);
                        command.Parameters.AddWithValue("@questionID", questionID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        QuestionModel model = new QuestionModel();
                        if (reader.Read())
                        {
                            model = BuildQuestionModel(reader);
                        }
                        return model;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionByID", ex);
                throw;
            }
        }
        public List<QuestionModel> GetNeceQuestionList(Guid queID)
        {
            bool nece = true;
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Questions
                    WHERE QueID = @queID AND Necessary = @Necessary
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@queID", queID);
                        command.Parameters.AddWithValue("@Necessary", nece);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<QuestionModel> list = new List<QuestionModel>();
                        while (reader.Read())
                        {
                            QuestionModel model = BuildQuestionModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetNeceQuestionList", ex);
                throw;
            }
        }
        public List<QuestionModel> GetQuestionList(Guid queID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Questions
                    WHERE QueID = @queID
                    ORDER BY QuestionNumber
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@queID", queID);

                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<QuestionModel> list = new List<QuestionModel>();
                        while (reader.Read())
                        {
                            QuestionModel model = BuildQuestionModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionList", ex);
                throw;
            }
        }
        public void CreateQuestion(QuestionModel model)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText = 
                $@"
                    INSERT INTO [Questions] (QueID, QuestionID, QueTitle, QueAns, Type, Necessary) 
                    VALUES (@QueID, @QuestionID, @QueTitle, @QueAns, @Type, @Necessary)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QueID", model.QueID);
                        command.Parameters.AddWithValue("@QuestionID", model.QuestionID);
                        command.Parameters.AddWithValue("@QueTitle", model.QueTitle);
                        command.Parameters.AddWithValue("@QueAns", model.QueAns);
                        command.Parameters.AddWithValue("@Type", model.Type);
                        command.Parameters.AddWithValue("@Necessary", model.Necessary);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog("QuestionManager.CreateQuestion", ex);
                throw;
            }
        }
        public void UpdateQuestion(QuestionModel model)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    UPDATE [Questions]
                    SET QueTitle = @QueTitle,
                        QueAns = @QueAns,
                        Type = @Type,
                        Necessary = @Necessary
                    WHERE QueID = @QueID AND QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QueID", model.QueID);
                        command.Parameters.AddWithValue("@QuestionID", model.QuestionID);
                        command.Parameters.AddWithValue("@QueTitle", model.QueTitle);
                        command.Parameters.AddWithValue("@QueAns", model.QueAns);
                        command.Parameters.AddWithValue("@Type", model.Type);
                        command.Parameters.AddWithValue("@Necessary", model.Necessary);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.UpdateQuestion", ex);
                throw;
            }
        }
        public void DeleteQuestion(Guid queID, Guid questionID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    DELETE FROM Questions
                    WHERE QueID = @QueID AND QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QueID", queID);
                        command.Parameters.AddWithValue("@QuestionID", questionID);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.DeleteQuestion", ex);
                throw;
            }
        }
        public void DeleteQuestion(Guid queID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    DELETE FROM Questions
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
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.DeleteQuestion", ex);
                throw;
            }
        }
        public QuestionModel BuildQuestionModel(SqlDataReader reader)
        {
            return new QuestionModel
            {
                QuestionNumber = (int)reader["QuestionNumber"],
                QueID = (Guid)reader["QueID"],
                QuestionID = (Guid)reader["QuestionID"],
                QueTitle = reader["QueTitle"] as string,
                QueAns = reader["QueAns"] as string,
                Type = (QueType)reader["Type"],
                Necessary = (bool)reader["Necessary"]
            };
        }
    }
}
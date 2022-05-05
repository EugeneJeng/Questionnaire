using Questionnaire.Helper;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Questionnaire.Managers
{
    public class CommonlyManager
    {
        public List<CommonlyModel> GetAllCommonly()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Commonlys
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<CommonlyModel> list = new List<CommonlyModel>();
                        while (reader.Read())
                        {
                            CommonlyModel model = BuildCommonlyModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetAllCommonly", ex);
                throw;
            }
        }
        public CommonlyModel GetCommonlyModel(Guid commID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Commonlys
                    WHERE QuestionID = @questionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@questionID", commID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        CommonlyModel model = new CommonlyModel();
                        if (reader.Read())
                        {
                            model = BuildCommonlyModel(reader);
                        }
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetAllCommonly", ex);
                throw;
            }
        }
        public void CreateCommonly(CommonlyModel model)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    INSERT INTO [Commonlys] (QuestionID, QueTitle, QueAns, Type) 
                    VALUES (@QuestionID, @QueTitle, @QueAns, @Type)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QuestionID", model.QuestionID);
                        command.Parameters.AddWithValue("@QueTitle", model.QueTitle);
                        command.Parameters.AddWithValue("@QueAns", model.QueAns);
                        command.Parameters.AddWithValue("@Type", model.Type);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonlyManager.CreateCommonly", ex);
                throw;
            }
        }
        public void UpdateCommonly(CommonlyModel model)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    UPDATE [Commonlys]
                    SET QueTitle = @QueTitle,
                        QueAns = @QueAns,
                        Type = @Type
                    WHERE QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QuestionID", model.QuestionID);
                        command.Parameters.AddWithValue("@QueTitle", model.QueTitle);
                        command.Parameters.AddWithValue("@QueAns", model.QueAns);
                        command.Parameters.AddWithValue("@Type", model.Type);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonlyManager.UpdateCommonly", ex);
                throw;
            }
        }
        public void DeleteCommonly(Guid questionID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    DELETE FROM Commonlys
                    WHERE QuestionID = @QuestionID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@QuestionID", questionID);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonlyManager.DeleteCommonly", ex);
                throw;
            }
        }
        public CommonlyModel BuildCommonlyModel(SqlDataReader reader)
        {
            return new CommonlyModel()
            {
                QuestionID = (Guid)reader["QuestionID"],
                QueTitle = reader["QueTitle"] as string,
                QueAns = reader["QueAns"] as string,
                Type = (QueType)reader["Type"],
            };
        }
    }
}
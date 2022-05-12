using Questionnaire.Models;
using Questionnaire.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Questionnaire.Managers
{
    public class QuestionnaireManager
    {
        public void CreateQuestionnaire(QuestionnaireModel model)
        {
            int state = (int)model.State;
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  INSERT INTO [Questionnaires]
                        (Title, QueID, StartTime, EndTime, QueContent, State, CreateDate )
                     VALUES 
                        (@Title, @QueID, @StartTime, @EndTime, @QueContent, @State, @CreateDate) ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@Title", model.Title);
                        command.Parameters.AddWithValue("@QueID", model.QueID);
                        command.Parameters.AddWithValue("@StartTime", model.StartTime);
                        command.Parameters.AddWithValue("@EndTime", model.EndTime);
                        command.Parameters.AddWithValue("@QueContent", model.QueContent);
                        command.Parameters.AddWithValue("@State", state);
                        command.Parameters.AddWithValue("@CreateDate", model.CreateDate);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.CreateQuestionnaire", ex);
                throw;
            }
        }
        public void UpdateQuestionnaire(QuestionnaireModel model)
        {
            //StateType state;
            //if (model.EndTime < DateTime.Now)
            //{
            //    state = StateType.已完結;
            //}
            //else
            //{
            //    state = StateType.投票中;
            //}
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"  UPDATE [Questionnaires]
                     SET Title = @title,
                         StartTime = @startDate,
                         EndTime = @endDate,
                         QueContent = @queContent,
                         State = @state
                     WHERE QueID = @id ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {                        
                        command.Parameters.AddWithValue("@title", model.Title);
                        command.Parameters.AddWithValue("@id", model.QueID);
                        command.Parameters.AddWithValue("@startDate", model.StartTime);
                        command.Parameters.AddWithValue("@endDate", model.EndTime);
                        command.Parameters.AddWithValue("@queContent", model.QueContent);
                        command.Parameters.AddWithValue("@state", (int)model.State);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.UpdateQuestionnaire", ex);
                throw;
            }
        }
        public List<QuestionnaireModel> GetQuestionnaireList()
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM [Questionnaires]
                    ORDER BY Number DESC
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<QuestionnaireModel> list = new List<QuestionnaireModel>();
                        while (reader.Read())
                        {
                            QuestionnaireModel model = BuildQuestionnaireModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaireList", ex);
                throw;
            }
        }
        public List<QuestionnaireModel> GetQuestionnaireList(int page)
        {
            int skip = 10 * (page - 1);
            if (skip < 0)
            {
                skip = 0;
            }
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT TOP 10 *
                    FROM [Questionnaires]
                    WHERE
                        QueID NOT IN
                        (
                            SELECT TOP {skip} QueID
                            FROM [Questionnaires]
                            ORDER BY Number DESC
                        )                        
                    ORDER BY Number DESC
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<QuestionnaireModel> list = new List<QuestionnaireModel>();
                        while (reader.Read())
                        {
                            QuestionnaireModel model = BuildQuestionnaireModel(reader);
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaireList", ex);
                throw;
            }
        }
        public List<QuestionnaireModel> GetQuestionnaireList(string keyword, DateTime? startDate, DateTime? endDate, int page)
        {
            string connStr = ConfigHelper.GetConnectionString();
            int skip = 10 * (page - 1);
            if (skip < 0)
            {
                skip = 0;
            }
            string whereCondition = string.Empty;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereCondition += "Title LIKE '%'+@keyword+'%'";
                if (!string.IsNullOrWhiteSpace(startDate.ToString()))
                {
                    whereCondition += " AND StartTime >= @startDate";
                }
                if (!string.IsNullOrWhiteSpace(endDate.ToString()))
                {
                    whereCondition += " AND EndTime <= @endDate";
                }
            }
            else if (!string.IsNullOrWhiteSpace(startDate.ToString()))
            {
                whereCondition += "StartTime >= @startDate";
                if (!string.IsNullOrWhiteSpace(endDate.ToString()))
                {
                    whereCondition += " AND EndTime <= @endDate";
                }
            }
            else if (!string.IsNullOrWhiteSpace(endDate.ToString()))
            {
                whereCondition += "EndTime <= @endDate";
            }
            string commandText =
                $@"
                    SELECT TOP 10 *
                    FROM [Questionnaires]
                    WHERE {whereCondition} AND QueID NOT IN
                        (
                            SELECT TOP {skip} QueID
                            FROM [Questionnaires]
                            WHERE {whereCondition}
                            ORDER BY Number DESC
                        )
                    ORDER BY Number DESC
                ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            command.Parameters.AddWithValue("@keyword", keyword);
                        }
                        if (!string.IsNullOrWhiteSpace(startDate.ToString()))
                        {
                            command.Parameters.AddWithValue("@startDate", startDate);
                        }
                        if (!string.IsNullOrWhiteSpace(endDate.ToString()))
                        {
                            command.Parameters.AddWithValue("@endDate", endDate);
                        }
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<QuestionnaireModel> list = new List<QuestionnaireModel>();
                        while (reader.Read())
                        {
                            QuestionnaireModel model = BuildQuestionnaireModel(reader);
                            //if (model.EndTime < DateTime.Now)
                            //{
                            //    model.State = StateType.已完結;
                            //}
                            //else
                            //{
                            //    model.State = StateType.投票中;
                            //}
                            list.Add(model);
                        }
                        return list;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaireList", ex);
                throw;
            }
        }
        public QuestionnaireModel GetQuestionnaireByQueID(Guid id)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =    
                $@"
                    SELECT *
                    FROM [Questionnaires]
                    WHERE QueID = @id
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        QuestionnaireModel model = new QuestionnaireModel();
                        if (reader.Read())
                        {
                            model = BuildQuestionnaireModel(reader);
                        }
                        return model;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaireByQueID", ex);
                throw;
            }
        }
        public void DeleteQuestionnaireByQueID(Guid id)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    DELETE FROM [Questionnaires]
                    WHERE QueID = @id
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog("DeleteQuestionnaireByQueID", ex);
                throw;
            }
        }
        public QuestionnaireModel BuildQuestionnaireModel(SqlDataReader reader)
        {
            return new QuestionnaireModel()
            {
                Number = (int)reader["Number"],
                Title = reader["Title"] as string,
                QueID = (Guid)reader["QueID"],
                StartTime = (DateTime)reader["StartTime"],
                EndTime = (DateTime)reader["EndTime"],
                CreateDate = (DateTime)reader["CreateDate"],
                QueContent = reader["QueContent"] as string,
                State = (StateType)reader["State"]
            };
        }
    }
}
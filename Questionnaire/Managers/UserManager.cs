using Questionnaire.Helper;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Questionnaire.Managers
{
    public class UserManager
    {
        public List<UserModel> GetUserList(Guid queID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Users
                    WHERE QueID = @queID
                    ORDER BY CreateDate
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
                        List<UserModel> list = new List<UserModel>();
                        while (reader.Read())
                        {
                            UserModel model = BuildUserModel(reader);
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
        public UserModel GetUser(Guid queID, Guid userID)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    SELECT *
                    FROM Users
                    WHERE QueID = @queID AND UserID = @userID
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@queID", queID);
                        command.Parameters.AddWithValue("@userID", userID);
                        conn.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        UserModel model = new UserModel();
                        if (reader.Read())
                        {
                            model = BuildUserModel(reader);
                        }
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.GetUser", ex);
                throw;
            }
        }
        public void CreateUser(UserModel user)
        {
            string connStr = ConfigHelper.GetConnectionString();
            string commandText =
                $@"
                    INSERT INTO [Users] (UserID, QueID, UserName, UserMail, UserPhone, UserAge, CreateDate) 
                    VALUES (@UserID, @QueID, @UserName, @UserMail, @UserPhone, @UserAge, @CreateDate)
                ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(commandText, conn))
                    {
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@QueID", user.QueID);
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@UserMail", user.UserMail);
                        command.Parameters.AddWithValue("@UserPhone", user.UserPhone);
                        command.Parameters.AddWithValue("@UserAge", user.UserAge); 
                        command.Parameters.AddWithValue("@CreateDate", user.CreateDate);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.CreateUser", ex);
                throw;
            }
        }
        public UserModel BuildUserModel(SqlDataReader reader)
        {
            return new UserModel()
            {
                UserID = (Guid)reader["UserID"],
                QueID = (Guid)reader["QueID"],
                UserName = reader["UserName"] as string,
                UserMail = reader["UserMail"] as string,
                UserPhone = reader["UserPhone"] as string,
                UserAge = (int)reader["UserAge"],
                CreateDate = (DateTime)reader["CreateDate"]
            };
        }
    }
}
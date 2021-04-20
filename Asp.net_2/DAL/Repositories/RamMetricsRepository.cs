﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Asp.net_2.Controllers;
using Asp.net_2.DAL.interfaces;

namespace Asp.net_2.DAL.Repositories
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection connection;
        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public RamMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }
        public void Create(RamMetrics item)
        {
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO rammetrics(value, time) VALUES(@value, @time)";
            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);
            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды// через свойство
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            // подготовка команды к выполнению
            cmd.Prepare();
            // выполнение команды
            cmd.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM rammetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        public void Update(RamMetrics item)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE rammetrics SET value = @value, time = @time WHERE id = @id; ";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        public IList<RamMetrics> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM rammetrics";
            var returnList = new List<RamMetrics>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new RamMetrics
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(0),// налету преобразуем прочитанные секунды в метку времени
                        Time = TimeSpan.FromSeconds(reader.GetInt32(0))
                    });
                }
            }
            return returnList;
        }
        public RamMetrics GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM rammetrics WHERE id=@id";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new RamMetrics
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(0),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(0))
                    };
                }
                else
                {
                    // не нашлось запись по идентификатору, не делаем ничего
                    return null;
                }
            }
        }
    }
}

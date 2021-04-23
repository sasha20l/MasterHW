
using Dapper;
using MetricsAgent.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DTO
{

    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке

    public class AllCpuMetricsResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }
    }
    public class SelectByTimePeriodCpuMetricsResponse : AllCpuMetricsResponse
    {
    }

    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {

    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        // наше соединение с базой данных
        private readonly SQLiteConnection _connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public CpuMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(CpuMetric item)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                // запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                // анонимный объект с параметрами запроса
                new
                {
                    // value подставится на место "@value" в строке запроса
                    // значение запишется из поля Value объекта item
                    value = item.Value,
                    // записываем в поле time количество секунд
                    time = item.Time
                });
            }
        }

        public IList<CpuMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<CpuMetric>("SELECT id, time, value FROM cpumetrics").ToList();
            }
        }
        public IList<CpuMetric> GetByTimePeriod(long getFromTime, long getToTime)
        {
            using (var connection = new SQLiteConnection(_connection))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
                    new { fromTime = getFromTime, toTime = getToTime }).ToList();
            }
        }
    }
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        void Create(T item);
        IList<T> GetByTimePeriod(long fromTime, long toTime);
    }

    public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value)
        => TimeSpan.FromSeconds((long)value);
        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
        => parameter.Value = value;
    }
}



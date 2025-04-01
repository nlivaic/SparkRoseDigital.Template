using System.Reflection;


namespace SparkRoseDigital_Template.WorkerServices
{
    public static class WorkerAssemblyInfo
    {
        public static Assembly Value { get; } = typeof(WorkerAssemblyInfo).Assembly;
        public static string ServiceName { get; } = "SparkRoseDigital_Template";
    }
}

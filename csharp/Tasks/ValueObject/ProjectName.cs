namespace Tasks.ValueObject
{
    public class ProjectName
    {
        private string Name { get; }

        public ProjectName(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
        
        /// <summary>
        /// 隱式轉換運算符
        /// </summary>
        public static implicit operator string(ProjectName obj) => obj.Name;
    }
}
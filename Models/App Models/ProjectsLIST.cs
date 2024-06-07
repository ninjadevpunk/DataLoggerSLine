namespace Data_Logger_1._3.Models.App_Models
{
    public class ProjectsLIST : List<ProjectClass>
    {

        public ProjectsLIST()
        {

        }

        public bool ContainsID(int ID)
        {

            foreach (var item in this)
            {
                if(item.ProjectID == ID)
                    return true;
            }

            return false;
        }
    }
}

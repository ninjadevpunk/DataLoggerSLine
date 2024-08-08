namespace Data_Logger_1._3.Models
{
    public class CheckList : List<CheckListItem>
    {

        /* CONSTRUCTORS */


        public CheckList() { }

        public CheckList(bool isChecked, string item)
        {
            this.Add(new CheckListItem(isChecked, item));
        }

        public CheckList(CheckListItem item)
        {
            this.Add(item);
        }



        /* MEMBER FUNCTIONS */



        public void Add(bool isChecked, string item)
        {
            this.Add(new CheckListItem(isChecked, item));
        }

    }
}

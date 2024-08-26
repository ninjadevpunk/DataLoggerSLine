using Data_Logger_1._3.Models;
using System.Data.SQLite;

namespace Data_Logger_1._3.Services
{
    public class DATAREADER : DATAMASTER
    {
        /*
         * DOCUMENTATION
         * 
         * The class for all database reading activities.
         * Please use this class for reading the database!
         * Designed for Analyst.
         */


        /* CONSTRUCTORS */


        public DATAREADER()
        {
            CreateConnection();
            CheckTables();
            CheckCategories();
            CheckApplications();
            CheckProject();
            CheckOutputs();
            CheckTypes();
            CheckSubjects();
            CheckMediums();
            CheckFormats();
            CheckUnits();
            CheckFNCategories();
        }

        public DATAREADER(bool status) : base(status) { }

        public DATAREADER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }

        // ---END CONSTRUCTORS---


        /* MEMBER FUNCTIONS */

        ///** Return the total number of LOGs **/
        //public int totalLOGS(string category)
        //{
        //    var temp = 0;

        //    var list = RetrieveLOGS(true);

        //    for (int i = 0; i < list.Count(); i++)
        //    {
        //        try
        //        {
        //            if (
        //                list.ElementAt(i).getCATEGORY() == category &&
        //                list.ElementAt(i).getApplicationName() != "Qt Creator" &&
        //                list.ElementAt(i).getApplicationName() != "Android Studio Electric Eel"
        //                )
        //            {
        //                ++temp;
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {

        //            //
        //        }
        //    }

        //    return temp;
        //}

        //public int totalLOGS(string category, string application)
        //{
        //    var temp = 0;

        //    var list = RetrieveLOGS(true);

        //    for (int i = 0; i < list.Count(); i++)
        //    {
        //        try
        //        {
        //            if (
        //            list.ElementAt(i).getCATEGORY() == category &&
        //            list.ElementAt(i).getApplicationName() == application
        //            )
        //            {
        //                ++temp;
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {

        //            //
        //        }
        //    }

        //    return temp;
        //}

        //public int totalLOGS(string category, string application, string project)
        //{
        //    var temp = 0;

        //    var list = RetrieveLOGS(true);

        //    for (int i = 0; i < list.Count(); i++)
        //    {
        //        try
        //        {
        //            if (
        //            list.ElementAt(i).getCATEGORY() == category &&
        //            list.ElementAt(i).getApplicationName() == application &&
        //            list.ElementAt(i).getProjectName() == project
        //            )
        //            {
        //                ++temp;
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {

        //            //
        //        }
        //    }

        //    return temp;
        //}


        ///** Return the total duration of a project. **/
        //public TimeSpan totalDuration(string app, string project)
        //{
        //    TimeSpan t1 = new TimeSpan(0, 0, 0, 0, 0);

        //    for (int i = 0; i < this.Count(); i++)
        //    {
        //        if (this.ElementAt(i).Application == app &&
        //            this.ElementAt(i).Project == project)
        //        {
        //            t1 += this.ElementAt(i).getDuration();
        //        }
        //    }

        //    return t1;
        //}

        ///** Return the total count of unsolved errors in a project. **/
        //public int xERtotalCount(string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    for (int i = 0; i < this.Count(); i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app &&
        //            this.ElementAt(i).getProjectName() == project)
        //        {
        //            for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()) &&
        //                    !xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution())
        //                    )
        //                {
        //                    ++count;
        //                }
        //            }
        //        }
        //    }

        //    return count;
        //}


        ///** Return the total count of errors in a LOG **/
        //public int ERtotalCount(string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    for (int i = 0; i < this.Count(); i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app &&
        //            this.ElementAt(i).getProjectName() == project)
        //        {
        //            for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()))
        //                {
        //                    ++count;
        //                }
        //            }
        //        }
        //    }

        //    return count;
        //}


        ///** Return the total count of solutions in a LOG **/
        //public int SOtotalCount(string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    for (int i = 0; i < this.Count(); i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app &&
        //            this.ElementAt(i).getProjectName() == project)
        //        {
        //            for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution()))
        //                {
        //                    ++count;
        //                }
        //            }
        //        }
        //    }

        //    return count;
        //}



        ///** Return the total count of suggestions in a LOG **/
        //public int SUtotalCount(string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app &&
        //            this.ElementAt(i).getProjectName() == project)
        //        {
        //            for (int j = 0; j < this.ElementAt(i).getNotarised().Count(); j++)
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSuggestion()))
        //                {
        //                    ++count;
        //                }
        //            }
        //        }
        //    }

        //    return count;
        //}




        ///** Return the total count of comments in a LOG **/
        //public int COtotalCount(string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app &&
        //            this.ElementAt(i).getProjectName() == project)
        //        {
        //            for (int j = 0; j < this.ElementAt(i).getNotarised().Count(); j++)
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getComment()))
        //                {
        //                    ++count;
        //                }
        //            }
        //        }
        //    }

        //    return count;
        //}



        ///** Find the highest/lowest amount of errors/solutions/suggestions/comments.
        // * Make sure only the logs from a particular app are included
        // *  If it's true (ASC) then return the lowest to highest.
        // *  If it's false (DESC) then return the highest to lowest.
        // *  Return a list with LOGS in the correct order.
        // *  */
        //public List<LOG> highORlow(bool asc_desc, string category, string app)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);
        //    Dictionary<LOG, int> array = new Dictionary<LOG, int>();


        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app)
        //        {
        //            for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //            {
        //                if (category == "ERROR")
        //                {
        //                    if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()))
        //                    {
        //                        ++count;
        //                    }
        //                }
        //                if (category == "SOLUTION")
        //                {
        //                    if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution()))
        //                    {
        //                        ++count;
        //                    }
        //                }
        //                if (category == "SUGGESTION")
        //                {
        //                    if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSuggestion()))
        //                    {
        //                        ++count;
        //                    }
        //                }
        //                if (category == "COMMENT")
        //                {
        //                    if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getComment()))
        //                    {
        //                        ++count;
        //                    }
        //                }
        //            }
        //            array.Add(this.ElementAt(i), count);
        //            count = 0;
        //        }
        //    }

        //    List<LOG> list = new List<LOG>();

        //    if (asc_desc)
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderBy(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }
        //    else
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderByDescending(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }



        //    return list;
        //}

        ///** Find the highest/lowest amount of errors/solutions/suggestions/comments.
        // * Make sure only the logs from a particular project are included
        // *  If it's true (ASC) then return the lowest to highest.
        // *  If it's false (DESC) then return the highest to lowest.
        // *  Return a list with LOGS in the correct order.
        // *  */
        //public List<LOG> highORlow(bool asc_desc, string category, string app, string project)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);
        //    Dictionary<LOG, int> array = new Dictionary<LOG, int>();


        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        if (this.ElementAt(i).getApplicationName() == app)
        //        {
        //            if (this.ElementAt(i).getProjectName() == project)
        //            {
        //                for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //                {
        //                    if (category == "ERROR")
        //                    {
        //                        if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()))
        //                        {
        //                            ++count;
        //                        }
        //                    }
        //                    if (category == "SOLUTION")
        //                    {
        //                        if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution()))
        //                        {
        //                            ++count;
        //                        }
        //                    }
        //                    if (category == "SUGGESTION")
        //                    {
        //                        if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSuggestion()))
        //                        {
        //                            ++count;
        //                        }
        //                    }
        //                    if (category == "COMMENT")
        //                    {
        //                        if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getComment()))
        //                        {
        //                            ++count;
        //                        }
        //                    }
        //                }
        //                array.Add(this.ElementAt(i), count);
        //                count = 0;
        //            }
        //        }
        //    }

        //    List<LOG> list = new List<LOG>();

        //    if (asc_desc)
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderBy(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }
        //    else
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderByDescending(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }

        //    return list;
        //}

        ///** Find the highest/lowest amount of errors/solutions/suggestions/comments.
        // *  If it's true (ASC) then return the lowest to highest.
        // *  If it's false (DESC) then return the highest to lowest.
        // *  Return a list with LOGS in the correct order.
        // *  */
        //public List<LOG> highORlow(bool asc_desc, string category)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);
        //    Dictionary<LOG, int> array = new Dictionary<LOG, int>();


        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //        {
        //            if (category == "ERROR")
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()))
        //                {
        //                    ++count;
        //                    continue;
        //                }
        //            }
        //            if (category == "SOLUTION")
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution()))
        //                {
        //                    ++count;
        //                    continue;
        //                }
        //            }
        //            if (category == "SUGGESTION")
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSuggestion()))
        //                {
        //                    ++count;
        //                    continue;
        //                }
        //            }
        //            if (category == "COMMENT")
        //            {
        //                if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getComment()))
        //                {
        //                    ++count;
        //                    continue;
        //                }
        //            }
        //        }
        //        array.Add(this.ElementAt(i), count);
        //        count = 0;
        //    }

        //    List<LOG> list = new List<LOG>();

        //    if (asc_desc)
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderBy(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }
        //    else
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderByDescending(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }



        //    return list;
        //}

        ///** Find the highest/lowest amount of errors without solutions.
        // *  If it's true (ASC) then return the lowest to highest.
        // *  If it's false (DESC) then return the highest to lowest.
        // *  Return a list with LOGS in the correct order.
        // *  */
        //public List<LOG> ERhighORlow(bool asc_desc)
        //{
        //    int count = 0;
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);
        //    Dictionary<LOG, int> array = new Dictionary<LOG, int>();


        //    for (int i = 0; i <= this.Count() - 1; i++)
        //    {
        //        for (int j = 0; j <= this.ElementAt(i).getNotarised().Count() - 1; j++)
        //        {
        //            if (xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getError()) &&
        //                !xp.IsMatch(this.ElementAt(i).getNotarised().ElementAt(j).getSolution())
        //                )
        //            {
        //                ++count;
        //            }
        //        }
        //        array.Add(this.ElementAt(i), count);
        //        count = 0;
        //    }

        //    List<LOG> list = new List<LOG>();

        //    if (asc_desc)
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderBy(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }
        //    else
        //    {
        //        foreach (KeyValuePair<LOG, int> v in array.OrderByDescending(key => key.Value))
        //        {
        //            list.Add(v.Key);
        //        }
        //    }



        //    return list;
        //}

        ///** Find the LOG in the DATAREADER list with a project name that matches proj1, and do the same for proj2.
        // *  Return a string that explains the differences or similarities between the 2.
        // *  
        // *  */
        //public string compare(string proj1, string proj2, string app, string category)
        //{
        //    /** 
        //     * variable1 is for the first project being evaluated
        //     * variable2 is for the second project being evaluated 
        //     *  */
        //    string temp = "";
        //    int index1 = -1, index2 = -1,
        //        count1 = 0, count2 = 0;
        //    bool exists1 = false, exists2 = false;

        //    if (proj1 != proj2)
        //    {
        //        for (int i = 0; i < this.Count(); i++)
        //        {
        //            if (this.ElementAt(i).getApplicationName() == app)
        //            {
        //                if (this.ElementAt(i).getProjectName() == proj1)
        //                {
        //                    exists1 = !exists1;
        //                    index1 = i;
        //                }
        //                else if (this.ElementAt(i).getProjectName() == proj2)
        //                {
        //                    exists2 = !exists2;
        //                    index2 = i;
        //                }
        //                else if (exists1 && exists2)
        //                    break; // No need to check the rest of the list if they have been found
        //            }
        //        }
        //    }


        //    if (exists1 && exists2)
        //    {
        //        // DURATION Comparison
        //        if (totalDuration(app, proj1) > totalDuration(app, proj2))
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() + " is " +
        //                (totalDuration(app, proj1) - totalDuration(app, proj2)).ToString() + " longer than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (totalDuration(app, proj1) == totalDuration(app, proj2))
        //        {
        //            temp += "There is no difference between these 2 projects in terms of duration. ";
        //        }
        //        else
        //        {
        //            temp += this.ElementAt(index1).getProjectName() + " is " +
        //                (totalDuration(app, proj2) - totalDuration(app, proj1)).ToString() +
        //                " shorter than " + this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        // TOTAL LOGS Comparison
        //        count1 = totalLOGS(category, app, proj1);

        //        count2 = totalLOGS(category, app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more logs than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "Both projects have " + count1.ToString() + " logs. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less logs than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }


        //        // UNSOLVED ERRORS Comparison
        //        count1 = xERtotalCount(app, proj1);

        //        count2 = xERtotalCount(app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more unsolved errors than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "These projects both have " + count1.ToString() + " unsolved errors. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less unsolved errors than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        // ERRORS
        //        count1 = ERtotalCount(app, proj1);

        //        count2 = ERtotalCount(app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more errors than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "These projects both have " + count1.ToString() + " errors. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less errors than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        count1 = 0;
        //        count2 = 0;

        //        // SOLUTIONS
        //        count1 = SOtotalCount(app, proj1);

        //        count2 = SOtotalCount(app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more solutions than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "These projects both have " + count1.ToString() + " solutions. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less solutions than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        count1 = 0;
        //        count2 = 0;

        //        // SUGGESTIONS
        //        count1 = SUtotalCount(app, proj1);

        //        count2 = SUtotalCount(app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more suggestions than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "These projects both have " + count1.ToString() + " suggestions. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less suggestions than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        count1 = 0;
        //        count2 = 0;


        //        // COMMENTS
        //        count1 = COtotalCount(app, proj1);

        //        count2 = COtotalCount(app, proj2);

        //        if (count1 > count2)
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count1 - count2).ToString() + " more comments than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }
        //        else if (count1 == count2)
        //        {
        //            temp += "These projects both have " + count1.ToString() + " comments. ";
        //        }
        //        else
        //        {
        //            temp += "The project " + this.ElementAt(index1).getProjectName() +
        //                " has " + (count2 - count1).ToString() + " less comments than the project " +
        //                this.ElementAt(index2).getProjectName() + ". ";
        //        }

        //        count1 = 0;
        //        count2 = 0;
        //    }
        //    else
        //        temp = "One of the projects (or both of them) do not exist!";

        //    return temp;
        //}

        //// TODO
        //public string compare(string proj1, string proj2, string proj3, string app, string category)
        //{
        //    string temp = "";

        //    return temp;
        //}

        /** Populates this DATAREADER with LOGS **/
        public void RetrieveLOGS()
        {
            this.Clear();

            List<LOG>? logs = null;
            bool Initialised = false;
            var note_IDs_LIST = new List<int>();
            var flexiLog_IDs_LIST = new List<int>();

            var categoryColumn = 1;
            var accountColumn = 2;
            var projectColumn = 3;
            var appColumn = 4;
            var startColumn = 5;
            var endColumn = 6;
            var outputColumn = 7;
            var typeColumn = 8;

            var noteLogTypeIDColumn = 1;

            SQLiteCommand query = _con.CreateCommand();


            query.CommandText = @"SELECT * FROM NotesLOG ORDER BY logID;";
            SQLiteDataReader read = query.ExecuteReader();

            read = query.ExecuteReader();


            while (read.Read())
            {

                if (!Initialised)
                {
                    logs = new();
                    Initialised = true;
                }
                else if (logs is not null)
                {
                    var type = FindNoteLogTypeByID(read.GetInt32(noteLogTypeIDColumn));
                    switch (type)
                    {
                        case NotesLOG.NOTELOGType.GENERIC:
                            {
                                NoteItem note = new();
                                note.ID = read.GetInt32(Column.IDColumn);
                                note_IDs_LIST.Add(note.ID);
                                logs.Add(note);

                                break;
                            }
                        case NotesLOG.NOTELOGType.FLEXI:
                            {
                                FlexiNotesLOG flexiLog = new();
                                flexiLog.ID = read.GetInt32(Column.IDColumn);
                                flexiLog_IDs_LIST.Add(flexiLog.ID);
                                logs.Add(flexiLog);

                                break;
                            }
                    }
                }

            }



            read.Close();


            query.CommandText = @"SELECT * FROM LOG ORDER BY logID;";



            while (read.Read() && logs is not null)
            {

                switch (FindCategoryByID(read.GetInt32(categoryColumn)))
                {
                    case LOG.CATEGORY.CODING:
                        {
                            CodingLOG codingLog = new();
                            codingLog.ID = read.GetInt32(Column.IDColumn);
                            codingLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                            codingLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                            codingLog.Application = FindAppByID(read.GetInt32(appColumn));
                            codingLog.Start = DateTime.Parse(read.GetString(startColumn));
                            codingLog.End = DateTime.Parse(read.GetString(endColumn));
                            codingLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                            codingLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                            codingLog.PostItList = new();

                            logs.Add(codingLog);

                            break;
                        }
                    case LOG.CATEGORY.GRAPHICS:
                        {
                            GraphicsLOG graphicsLog = new();
                            graphicsLog.ID = read.GetInt32(Column.IDColumn);
                            graphicsLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                            graphicsLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                            graphicsLog.Application = FindAppByID(read.GetInt32(appColumn));
                            graphicsLog.Start = DateTime.Parse(read.GetString(startColumn));
                            graphicsLog.End = DateTime.Parse(read.GetString(endColumn));
                            graphicsLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                            graphicsLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                            graphicsLog.PostItList = new();


                            break;
                        }
                    case LOG.CATEGORY.FILM:
                        {
                            FilmLOG filmLog = new();
                            filmLog.ID = read.GetInt32(Column.IDColumn);
                            filmLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                            filmLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                            filmLog.Application = FindAppByID(read.GetInt32(appColumn));
                            filmLog.Start = DateTime.Parse(read.GetString(startColumn));
                            filmLog.End = DateTime.Parse(read.GetString(endColumn));
                            filmLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                            filmLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                            filmLog.PostItList = new();

                            break;
                        }
                    case LOG.CATEGORY.NOTES:
                        {
                            foreach (LOG log in logs)
                            {
                                if (read.GetInt32(Column.IDColumn) == log.ID)
                                {
                                    log.Author = FindAccountByID(read.GetInt32(accountColumn));
                                    log.Project = FindProjectByID(read.GetInt32(projectColumn));
                                    log.Application = FindAppByID(read.GetInt32(appColumn));
                                    log.Start = DateTime.Parse(read.GetString(startColumn));
                                    log.End = DateTime.Parse(read.GetString(endColumn));
                                    log.Output = FindOutputByID(read.GetInt32(outputColumn));
                                    log.Type = FindTypeByID(read.GetInt32(typeColumn));
                                    log.PostItList = new();

                                    break;
                                }
                            }
                            break;

                        }
                }
            }

            read.Close();




            query.CommandText = @"SELECT * FROM POSTIT ORDER BY logID ASC, postItID ASC;";
            read = query.ExecuteReader();

            var logIDColumn = 1;
            var subjectColumn = 2;
            var errorColumn = 3;
            var solutionColumn = 5;
            var suggestionColumn = 7;
            var commentColumn = 8;
            var foundColumn = 4;
            var solvedColumn = 6;

            while (read.Read() && logs is not null)
            {

                foreach (LOG log in logs)
                {
                    if (read.GetInt32(logIDColumn) == log.ID)
                    {
                        log.PostItList.Add(new(read.GetInt32(Column.IDColumn), FindSubjectByID(read.GetInt32(subjectColumn)), read.GetString(errorColumn),
                            read.GetString(solutionColumn), read.GetString(suggestionColumn), read.GetString(commentColumn), DateTime.Parse(read.GetString(foundColumn)),
                            DateTime.Parse(read.GetString(solvedColumn))));

                        break;
                    }
                }

            }

            read.Close();


            foreach (LOG log in logs)
            {
                switch (log.Category)
                {
                    case LOG.CATEGORY.CODING:
                        {
                            RetrieveCodingLOG((CodingLOG)log);

                            break;
                        }
                    case LOG.CATEGORY.GRAPHICS:
                        {
                            RetrieveGraphicsLOG((GraphicsLOG)log);

                            break;
                        }
                    case LOG.CATEGORY.FILM:
                        {
                            RetrieveFilmLOG((FilmLOG)log);

                            break;
                        }
                    case LOG.CATEGORY.NOTES:
                        {
                            if (note_IDs_LIST.Contains(log.ID))
                                RetrieveNoteItem((NoteItem)log);
                            else if (flexiLog_IDs_LIST.Contains(log.ID))
                                RetrieveFlexiNotesLOG((FlexiNotesLOG)log);

                            break;
                        }
                }
            }

        }

        public void RetrieveCodingLOG(CodingLOG codingLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM CodingLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", codingLog.ID);

                var bugsColumn = 1;
                var openedColumn = 2;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    codingLog.Bugs = read.GetInt32(bugsColumn);
                    codingLog.Success = read.GetBoolean(openedColumn);
                }

                if (codingLog.Application.Name == Android)
                    RetrieveAndroidCodingLOG((AndroidCodingLOG)codingLog);
                else
                    this.Add(codingLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveAndroidCodingLOG(AndroidCodingLOG androidCodingLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM AndroidCodingLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", androidCodingLog.ID);

                var fullORsimple = 1;
                var syncColumn = 2;
                var gradleColumn = 3;
                var runBuildColumn = 4;
                var loadBuildColumn = 5;
                var configBuildColumn = 6;
                var allProjectsColumn = 7;


                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    androidCodingLog.Scope = read.GetBoolean(fullORsimple) ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;
                    androidCodingLog.Sync = DateTime.Parse(read.GetString(syncColumn));
                    androidCodingLog.StartingGradleDaemon = DateTime.Parse(read.GetString(gradleColumn));
                    androidCodingLog.RunBuild = DateTime.Parse(read.GetString(runBuildColumn));
                    androidCodingLog.LoadBuild = DateTime.Parse(read.GetString(loadBuildColumn));
                    androidCodingLog.ConfigureBuild = DateTime.Parse(read.GetString(configBuildColumn));
                    androidCodingLog.AllProjects = DateTime.Parse(read.GetString(allProjectsColumn));
                }

                read.Close();


                this.Add(androidCodingLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveGraphicsLOG(GraphicsLOG graphicsLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM GraphicsLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", graphicsLog.ID);

                var mediumColumn = 1;
                var formatColumn = 2;
                var brushColumn = 3;
                var heightColumn = 4;
                var widthColumn = 5;
                var unitColumn = 6;
                var sizeColumn = 7;
                var DPIColumn = 8;
                var depthColumn = 9;
                var IsCompletedColumn = 10;
                var sourceColumn = 11;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    graphicsLog.Medium = read.GetString(mediumColumn);
                    graphicsLog.Format = read.GetString(formatColumn);
                    graphicsLog.Brush = read.GetString(brushColumn);
                    graphicsLog.Height = read.GetDouble(heightColumn);
                    graphicsLog.Width = read.GetDouble(widthColumn);
                    graphicsLog.Unit = read.GetString(unitColumn);
                    graphicsLog.Size = read.GetString(sizeColumn);
                    graphicsLog.DPI = read.GetDouble(DPIColumn);
                    graphicsLog.Depth = read.GetString(depthColumn);
                    graphicsLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    graphicsLog.Source = read.GetString(sourceColumn);

                }

                read.Close();

                this.Add(graphicsLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveFilmLOG(FilmLOG filmLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM FilmLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", filmLog.ID);

                var heightColumn = 1;
                var widthColumn = 2;
                var lengthColumn = 3;
                var IsCompletedColumn = 4;
                var sourceColumn = 5;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    filmLog.Height = read.GetDouble(heightColumn);
                    filmLog.Width = read.GetDouble(widthColumn);
                    filmLog.Length = read.GetString(lengthColumn);
                    filmLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    filmLog.Source = read.GetString(sourceColumn);
                }

                read.Close();

                this.Add(filmLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveNoteItem(NoteItem note)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM NoteItem WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", note.ID);
                bool IsChecklist = false;

                var IsChecklistColumn = 1;
                var subjectColumn = 2;
                var noteColumn = 3;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    IsChecklist = read.GetBoolean(IsChecklistColumn);
                    note.Items = IsChecklist ? new() : null;
                    note.Subject = read.GetString(subjectColumn);
                    note.Content = read.GetString(noteColumn);
                }

                read.Close();

                var itemColumn = 2;
                var doneColumn = 3;


                if (IsChecklist)
                {
                    query.CommandText = @"SELECT * FROM Checklist WHERE logID = @id ORDER BY logID;";
                    query.Parameters.AddWithValue("@id", note.ID);

                    read = query.ExecuteReader();

                    while (read.Read() && note.Items is not null)
                    {
                        note.Items.Add(new CheckListItem(read.GetInt32(Column.IDColumn), read.GetBoolean(doneColumn), read.GetString(itemColumn)));
                    }
                }

                this.Add(note);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveFlexiNotesLOG(FlexiNotesLOG flexiLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM FlexiNotesLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", flexiLog.ID);

                var flexiNoteTypeIDColumn = 1;
                var mediumColumn = 2;
                var formatColumn = 3;
                var bitRateColumn = 4;
                var lengthColumn = 5;
                var IsCompletedColumn = 6;
                var sourceColumn = 7;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    flexiLog.flexinotetype = FindFlexiNoteTypeByID(flexiNoteTypeIDColumn);
                    flexiLog.Medium = FindMediumByID(read.GetInt32(mediumColumn));
                    flexiLog.Format = FindFormatByID(read.GetInt32(formatColumn));
                    flexiLog.Bitrate = read.GetInt32(bitRateColumn);
                    flexiLog.Length = read.GetString(lengthColumn);
                    flexiLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    flexiLog.Source = read.GetString(sourceColumn);
                }

                read.Close();

                this.Add(flexiLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }


        //public List<LOG> RetrieveLOGS(bool theListONLY)
        //{
        //    /** Retrieve the LOGs from the database and then use the PROJECT's ID to find the app and category.
        //     * Use the CATEGORY ID to find the correct info
        //     */
        //    int f = 0, accountID = 0;
        //    var list = new List<LOG>();

        //    // IDs
        //    List<int> ids = new List<int>(),
        //        acIDs = new List<int>(),
        //        proIDs = new List<int>(),
        //        appIDs = new List<int>(),
        //        catIDs = new List<int>(),
        //        outputIDs = new List<int>(),
        //        typeIDs = new List<int>();

        //    Dictionary<int, int> mediumIDs = new Dictionary<int, int>(), // logID and mediumID
        //        formatIDs = new Dictionary<int, int>(), // logID and formatID
        //        unitIDs = new Dictionary<int, int>(), // logID and unitID
        //        FNCategoryIDs = new Dictionary<int, int>(); // logID and FNCategoryID


        //    ACCOUNT account = new ACCOUNT();
        //    List<string> proList = new List<string>();
        //    List<string> appList = new List<string>();
        //    List<DateTime> startList = new List<DateTime>();
        //    List<DateTime> endList = new List<DateTime>();
        //    List<string> outputList = new List<string>();
        //    List<string> typeList = new List<string>();
        //    List<List<Notary>> notaries = new List<List<Notary>>();
        //    Dictionary<int, string> subPairs = new Dictionary<int, string>();

        //    (int, string) mediumList = (0, "");
        //    (int, string) formatList = (0, "");
        //    (int, string) unitList = (0, "");
        //    (int, string) FNCategoryList = (0, "");

        //    SQLiteCommand query = _con.CreateCommand();
        //    query.CommandText = "SELECT * FROM ACCOUNT WHERE status = @status LIMIT 1;";

        //    query.Parameters.AddWithValue("@status", 1);
        //    SQLiteDataReader read;

        //    /** Add the account to a List<ACCOUNT> so they can be added to the DATAREADER List LOGs
        //    *   To find it just look at who's online and add that ONE ACCOUNT to the "account" variable.
        //    *   */

        //    read = query.ExecuteReader();

        //    while (read.Read())
        //    {
        //        accountID = read.GetInt32(0);

        //        account.setDisplayPicture(read.GetString(1));
        //        account.setFirstName(read.GetString(2));
        //        account.setLastName(read.GetString(3));
        //        var email = new SecureString();

        //        foreach (char c in read.GetString(4).ToCharArray())
        //        {
        //            email.AppendChar(c);
        //        }
        //        account.setEmail(email);
        //        account.setPassword(new SecureString());
        //        account.setStatus(true);
        //    }

        //    read.Close();

        //    query.CommandText = "SELECT * FROM LOG WHERE accountID = @id ORDER BY logID";

        //    query.Parameters.AddWithValue("@id", accountID);

        //    read = query.ExecuteReader();

        //    while (read.Read())
        //    {
        //        ids.Add(read.GetInt32(0)); // LOG ID
        //        proIDs.Add(read.GetInt32(2)); // PROJECT ID
        //        startList.Add(DateTime.Parse(read.GetString(3))); // Start Times
        //        endList.Add(DateTime.Parse(read.GetString(4))); // End Times
        //        outputIDs.Add(read.GetInt32(5)); // OUTPUT ID
        //        typeIDs.Add(read.GetInt32(6)); // TYPE ID
        //    }

        //    read.Close();

        //    /** Get the Notaries from the LOG_NOTE table **/
        //    /** Check that the LOG_NOTE has valid input and not empty strings */
        //    string pattern = "[A-Za-z]{5}[0-9]{0}";
        //    Regex xp = new Regex(pattern);

        //    int logIDb4 = 0; // The previous record's note's logID
        //    var noteList = new List<Notary>(); // Temporary list for adding to LOG Notary member variable
        //    query.CommandText = "SELECT * FROM LOG_NOTE ORDER BY logID ASC, subjectID ASC;";

        //    read = query.ExecuteReader();
        //    subPairs = retrieveSubjects();
        //    var temp = true;

        //    /** Start a new Notary for each record. */
        //    Notary thought;


        //    while (read.Read())
        //    {
        //        thought = new Notary();

        //        if (temp)
        //        {
        //            logIDb4 = read.GetInt32(1);
        //            temp = false;
        //        }

        //        if (ids.Contains(read.GetInt32(1)))
        //        {

        //            /** Check to see if we're dealing with a new List of Notoaries.
        //             *  You also need to create a new List<Notary> because the logID has changed. Each log deals with one List of type Notary!
        //             *  */
        //            if (logIDb4 != read.GetInt32(1))
        //            {
        //                notaries.Add(noteList);
        //                noteList = new List<Notary>();
        //            }

        //            if (subPairs.Count > 0)
        //            {
        //                for (int j = 0; j < subPairs.Count; j++)
        //                {
        //                    if (read.GetInt32(2) == subPairs.ElementAt(j).Key)
        //                    {
        //                        thought.setSubject(subPairs.Values.ElementAt(j));
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                thought.setSubject("No Subject");
        //            }

        //            // ERROR
        //            if (xp.IsMatch(read.GetString(3)))
        //            {
        //                thought.setError(read.GetString(3));
        //                thought.setERCaptureTime(DateTime.Parse(read.GetString(4)));
        //            }

        //            // SOLUTION
        //            if (xp.IsMatch(read.GetString(5)))
        //            {
        //                thought.setSolution(read.GetString(5));
        //                thought.setSOCaptureTime(DateTime.Parse(read.GetString(6)));
        //            }

        //            // SUGGESTION
        //            if (xp.IsMatch(read.GetString(7)))
        //                thought.setSuggestion(read.GetString(7));

        //            // COMMENT
        //            if (xp.IsMatch(read.GetString(8)))
        //                thought.setComment(read.GetString(8));

        //        }
        //        else
        //        {
        //            notaries.Add(new List<Notary>());
        //            noteList = new List<Notary>();
        //        }

        //        noteList.Add(thought);
        //        logIDb4 = read.GetInt32(1);
        //    }

        //    while (notaries.Count != ids.Count)
        //    {
        //        if (notaries.Count < ids.Count)
        //        {
        //            notaries.Add(new List<Notary>());
        //        }


        //    }

        //    read.Close();


        //    /** Do what you did for account but for PROJECTs.
        //     *  Make sure to add the projects to proList
        //     *  You can add the app IDs to appIDs simultaneously
        //     *  */
        //    query.CommandText = "SELECT * FROM PROJECT ORDER BY projectID";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] parray = new string[ids.Count];
        //    int[] arrayID = new int[ids.Count];

        //    while (read.Read())
        //    {
        //        if (proIDs.Count > 0)
        //        {
        //            for (f = 0; f < ids.Count; f++)
        //            {
        //                if (read.GetInt32(0) == proIDs.ElementAt(f))
        //                {
        //                    parray[f] = read.GetString(1); // Project Names
        //                    arrayID[f] = read.GetInt32(2); // appIDs
        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < parray.Length; i++)
        //    {
        //        proList.Add(parray[i]);
        //        appIDs.Add(arrayID[i]);
        //    }

        //    read.Close();

        //    /** Retrieve apps **/
        //    query.CommandText = "SELECT * FROM APPLICATION ORDER BY categoryID ASC, appID ASC";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] array = new string[ids.Count];
        //    int[] carrayID = new int[ids.Count];

        //    while (read.Read())
        //    {
        //        if (appIDs.Count > 0)
        //        {
        //            for (f = 0; f < ids.Count; f++)
        //            {
        //                if (read.GetInt32(0) == appIDs.ElementAt(f))
        //                {
        //                    array[f] = read.GetString(1); // App Names
        //                    carrayID[f] = read.GetInt32(2); // Category IDs
        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        appList.Add(array[i]);
        //        catIDs.Add(carrayID[i]);
        //    }

        //    read.Close();

        //    /** Retrieve Outputs */
        //    query.CommandText = "SELECT * FROM OUTPUT ORDER BY outputID;";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] orray = new string[ids.Count];

        //    while (read.Read())
        //    {
        //        if (outputIDs.Count > 0)
        //        {
        //            for (f = 0; f < ids.Count; f++)
        //            {
        //                if (read.GetInt32(0) == outputIDs.ElementAt(f))
        //                {
        //                    orray[f] = read.GetString(1); // Outputs
        //                }
        //            }
        //        }
        //    }

        //    read.Close();

        //    /** Retrieve Outputs */
        //    query.CommandText = "SELECT * FROM TYPE ORDER BY typeID;";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] trray = new string[ids.Count];

        //    while (read.Read())
        //    {
        //        if (typeIDs.Count > 0)
        //        {
        //            for (f = 0; f < ids.Count; f++)
        //            {
        //                if (read.GetInt32(0) == typeIDs.ElementAt(f))
        //                {
        //                    trray[f] = read.GetString(1); // Types
        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < ids.Count; i++)
        //    {
        //        outputList.Add(orray[i]);
        //        typeList.Add(trray[i]);
        //    }

        //    read.Close();

        //    /** Make sure to add the mediumIDs, formatIDs and unitIDs in a way that will make them easy to extract through a for loop. **/
        //    query.CommandText = "SELECT * FROM GRAPHICS_LOG ORDER BY logID ASC, mediumID ASC;";
        //    read = query.ExecuteReader();

        //    while (read.Read())
        //    {
        //        mediumIDs.Add(read.GetInt32(0), read.GetInt32(1));
        //        formatIDs.Add(read.GetInt32(0), read.GetInt32(2));
        //        unitIDs.Add(read.GetInt32(0), read.GetInt32(6));
        //    }

        //    read.Close();

        //    /** Make sure to add the unitIDs in a way that will make them easy to extract through a for loop **/
        //    query.CommandText = "SELECT * FROM FILM_LOG ORDER BY logID;";
        //    read = query.ExecuteReader();

        //    while (read.Read())
        //    {
        //        unitIDs.Add(read.GetInt32(0), read.GetInt32(3));
        //    }

        //    read.Close();

        //    /** Make sure to add the FNCategories, mediumIDs and formatIDs in a way that will make them easy to extract 
        //     * through a for loop. **/
        //    query.CommandText = "SELECT * FROM FLEXI_NOTE_LOG ORDER BY logID;";
        //    read = query.ExecuteReader();

        //    while (read.Read())
        //    {
        //        FNCategoryIDs.Add(read.GetInt32(0), read.GetInt32(1));
        //        mediumIDs.Add(read.GetInt32(0), read.GetInt32(2));
        //        formatIDs.Add(read.GetInt32(0), read.GetInt32(3));
        //    }

        //    read.Close();

        //    query.CommandText = "SELECT * FROM MEDIUM ORDER BY mediumID ASC;";
        //    read = query.ExecuteReader();
        //    string[] marray = new string[ids.Count];


        //    while (read.Read())
        //    {
        //        for (f = 0; f < mediumIDs.Count; f++)
        //        {
        //            if (read.GetInt32(0) == mediumIDs.ElementAt(f).Value)
        //            {
        //                for (int i = 0; i < ids.Count; i++)
        //                {
        //                    marray[i] = "";
        //                    if (ids.ElementAt(i) == mediumIDs.ElementAt(f).Key)
        //                        marray[i] = read.GetString(1);

        //                }

        //            }
        //        }
        //    }

        //    read.Close();

        //    query.CommandText = "SELECT * FROM FORMAT ORDER BY formatID ASC;";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] farray = new string[ids.Count];

        //    while (read.Read())
        //    {
        //        for (f = 0; f < formatIDs.Count; f++)
        //        {
        //            if (read.GetInt32(0) == formatIDs.ElementAt(f).Value)
        //            {
        //                for (int i = 0; i < ids.Count; i++)
        //                {
        //                    farray[i] = "";
        //                    if (ids.ElementAt(i) == formatIDs.ElementAt(f).Key)
        //                        farray[i] = read.GetString(1);

        //                }

        //            }
        //        }
        //    }

        //    read.Close();

        //    query.CommandText = "SELECT * FROM MEASURING_UNIT ORDER BY unitID ASC;";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] urray = new string[ids.Count];

        //    while (read.Read())
        //    {
        //        for (f = 0; f < unitIDs.Count; f++)
        //        {
        //            if (read.GetInt32(0) == unitIDs.ElementAt(f).Value)
        //            {
        //                for (int i = 0; i < ids.Count; i++)
        //                {
        //                    urray[i] = "";
        //                    if (ids.ElementAt(i) == unitIDs.ElementAt(f).Key)
        //                        urray[i] = read.GetString(1);

        //                }

        //            }
        //        }
        //    }

        //    read.Close();

        //    for (f = 0; f < mediumIDs.Count; f++)
        //    {
        //        for (int i = 0; i < marray.Length; i++)
        //        {
        //            mediumList = (mediumIDs.ElementAt(f).Key, marray[i]);
        //            formatList = (formatIDs.ElementAt(f).Key, farray[i]);
        //            unitList = (unitIDs.ElementAt(f).Key, urray[i]);
        //        }
        //    }

        //    query.CommandText = "SELECT * FROM FLEXI_NOTE_CAT ORDER BY flexiNoteID ASC;";
        //    read = query.ExecuteReader();
        //    f = 0;
        //    string[] fnarray = new string[ids.Count];

        //    while (read.Read())
        //    {
        //        for (f = 0; f < FNCategoryIDs.Count; f++)
        //        {
        //            if (read.GetInt32(0) == FNCategoryIDs.ElementAt(f).Value)
        //            {
        //                for (int i = 0; i < ids.Count; i++)
        //                {
        //                    fnarray[i] = "";
        //                    if (ids.ElementAt(i) == FNCategoryIDs.ElementAt(f).Key)
        //                        fnarray[i] = read.GetString(1);

        //                }

        //            }
        //        }
        //    }

        //    read.Close();

        //    for (f = 0; f < FNCategoryIDs.Count; f++)
        //    {
        //        for (int i = 0; i < fnarray.Length; i++)
        //        {
        //            FNCategoryList = (FNCategoryIDs.ElementAt(f).Key, fnarray[i]);
        //        }
        //    }


























        //    // Read the data for specific LOG.
        //    for (int i = 0; i < ids.Count; i++)
        //    {
        //        switch (catIDs.ElementAt(i))
        //        {
        //            case 1:
        //                {
        //                    query.CommandText = "SELECT * FROM CODE_LOG ORDER BY logID;";
        //                    read = query.ExecuteReader();

        //                    var clog = new CODE_LOG(account,
        //                        proList.ElementAt(i), appList.ElementAt(i), startList.ElementAt(i), endList.ElementAt(i),
        //                        outputList.ElementAt(i), typeList.ElementAt(i), notaries.ElementAt(i), 0, false);

        //                    while (read.Read())
        //                    {
        //                        // If the logID is the same as the current logID set log contents
        //                        if (ids.ElementAt(i) == read.GetInt32(0) && i < ids.Count)
        //                        {
        //                            clog.setBugs(read.GetInt32(1));
        //                            clog.setSuccess(read.GetBoolean(2));
        //                            break;
        //                        }
        //                    }

        //                    read.Close();

        //                    if (clog.getApplicationName() == "Android Studio Electric Eel")
        //                    {
        //                        query.CommandText = "SELECT * FROM ANDROID_CODE_LOG ORDER BY logID";
        //                        read = query.ExecuteReader();
        //                        var acl = new ANDROID_CODE_LOG(clog);

        //                        while (read.Read())
        //                        {
        //                            if (ids.ElementAt(i) == read.GetInt32(0))
        //                            {
        //                                acl.setSync(DateTime.Parse(read.GetString(1)));

        //                                if (read.GetString(2) != null)
        //                                    acl.setGradleDaemon(DateTime.Parse(read.GetString(2)));

        //                                if (read.GetString(3) != null)
        //                                    acl.setBuildRun(DateTime.Parse(read.GetString(3)));

        //                                if (read.GetString(4) != null)
        //                                    acl.setBuildLoad(DateTime.Parse(read.GetString(4)));

        //                                if (read.GetString(5) != null)
        //                                    acl.setBuildConfig(DateTime.Parse(read.GetString(5)));

        //                                if (read.GetString(6) != null)
        //                                    acl.setAllProjects(DateTime.Parse(read.GetString(6)));
        //                                break;
        //                            }
        //                        }

        //                        read.Close();

        //                        if (theListONLY)
        //                            list.Add(acl);
        //                        else
        //                            this.Add(acl);
        //                    }
        //                    else
        //                    {
        //                        if (theListONLY)
        //                            list.Add(clog);
        //                        else
        //                            this.Add(clog);
        //                    }

        //                }
        //                break;
        //            case 2:
        //                {
        //                    var glog = new GRAPHICS_LOG();

        //                    glog.setAuthor(account); // ACCOUNT
        //                    glog.setProjectName(proList.ElementAt(i)); // PROJECT
        //                    glog.setApplicationName(appList.ElementAt(i)); // APPLICATION
        //                    glog.setStartTime(startList.ElementAt(i)); // START TIME
        //                    glog.setEndTime(endList.ElementAt(i)); // END TIME
        //                    glog.setOutput(outputList.ElementAt(i)); // OUTPUT
        //                    glog.setType(typeList.ElementAt(i)); // TYPE
        //                    glog.setNotarised(notaries.ElementAt(i)); // Notaries

        //                    query.CommandText = "SELECT * FROM GRAPHICS_LOG ORDER BY logID;";
        //                    read = query.ExecuteReader();

        //                    while (read.Read())
        //                    {
        //                        // If the logID is the same as the current logID set log contents
        //                        if (ids.ElementAt(i) == read.GetInt32(0))
        //                        {
        //                            if (mediumList.Item1 == read.GetInt32(1))
        //                                glog.setMedium(mediumList.Item2);

        //                            if (formatList.Item1 == read.GetInt32(2))
        //                                glog.setFormat(mediumList.Item2);

        //                            glog.setBrush(read.GetString(3));
        //                            glog.setHeight(read.GetDecimal(4));
        //                            glog.setWidth(read.GetDecimal(5));

        //                            if (unitList.Item1 == read.GetInt32(6))
        //                                glog.setUnit(unitList.Item2);

        //                            glog.setSize(read.GetString(7));
        //                            glog.setDPI(read.GetDecimal(8));
        //                            glog.setDepth(read.GetString(9));
        //                            glog.setCompleted(read.GetBoolean(10));
        //                            glog.setSource(read.GetString(11));
        //                            break;
        //                        }
        //                    }
        //                    read.Close();

        //                    if (theListONLY)
        //                        list.Add(glog);
        //                    else
        //                        this.Add(glog);
        //                }
        //                break;
        //            case 3:
        //                {
        //                    var flog = new FILM_LOG();

        //                    flog.setAuthor(account); // ACCOUNT
        //                    flog.setProjectName(proList.ElementAt(i)); // PROJECT
        //                    flog.setApplicationName(appList.ElementAt(i)); // APPLICATION
        //                    flog.setStartTime(startList.ElementAt(i)); // START TIME
        //                    flog.setEndTime(endList.ElementAt(i)); // END TIME
        //                    flog.setOutput(outputList.ElementAt(i)); // OUTPUT
        //                    flog.setType(typeList.ElementAt(i)); // TYPE
        //                    flog.setNotarised(notaries.ElementAt(i)); // Notaries

        //                    query.CommandText = "SELECT * FROM FILM_LOG ORDER BY logID;";
        //                    read = query.ExecuteReader();

        //                    while (read.Read())
        //                    {
        //                        // If the logID is the same as the current logID set log contents
        //                        if (ids.ElementAt(i) == read.GetInt32(0))
        //                        {
        //                            flog.setHeight(read.GetDecimal(1));
        //                            flog.setWidth(read.GetDecimal(2));

        //                            if (unitList.Item1 == read.GetInt32(3))
        //                                flog.setUnit(unitList.Item2);

        //                            flog.setLength(read.GetString(4));
        //                            flog.setCompleted(read.GetBoolean(5));
        //                            flog.setSource(read.GetString(6));

        //                            break;
        //                        }
        //                    }
        //                    read.Close();

        //                    if (theListONLY)
        //                        list.Add(flog);
        //                    else
        //                        this.Add(flog);
        //                }
        //                break;
        //            case 4:
        //                {
        //                    query.CommandText = "SELECT * FROM NOTE_LOG ORDER BY logID;";
        //                    read = query.ExecuteReader();
        //                    bool genORflex = true;

        //                    while (read.Read())
        //                    {
        //                        if (ids.ElementAt(i) == read.GetInt32(0))
        //                        {
        //                            genORflex = read.GetBoolean(1);
        //                            break;
        //                        }
        //                    }

        //                    read.Close();


        //                    if (genORflex)
        //                    {
        //                        bool isList = true;
        //                        var gen = new GENERIC_NOTE();

        //                        gen.setAuthor(account); // ACCOUNT
        //                        gen.setProjectName(proList.ElementAt(i)); // PROJECT
        //                        gen.setApplicationName(appList.ElementAt(i)); // APPLICATION
        //                        gen.setStartTime(startList.ElementAt(i)); // START TIME
        //                        gen.setEndTime(endList.ElementAt(i)); // END TIME
        //                        gen.setOutput(outputList.ElementAt(i)); // OUTPUT
        //                        gen.setType(typeList.ElementAt(i)); // TYPE

        //                        query.CommandText = "SELECT * FROM GENERIC_NOTE_LOG ORDER BY logID;";
        //                        read = query.ExecuteReader();

        //                        while (read.Read())
        //                        {
        //                            // If the logID is the same as the current logID set log contents
        //                            if (ids.ElementAt(i) == read.GetInt32(0))
        //                            {
        //                                gen.setCategory(read.GetBoolean(1));
        //                                isList = read.GetBoolean(1);
        //                                gen.setSubject(read.GetString(2));
        //                                gen.setNote(read.GetString(3));

        //                                break;
        //                            }
        //                        }

        //                        read.Close();


        //                        f = 0;
        //                        if (!isList)
        //                        {
        //                            query.CommandText = "SELECT * FROM GNL_CHECKLIST ORDER BY gnlCheckID ASC, logID ASC;";
        //                            read = query.ExecuteReader();
        //                            Checklist dict = new Checklist();

        //                            while (read.Read())
        //                            {
        //                                if (ids.ElementAt(i) == read.GetInt32(1))
        //                                {
        //                                    dict.Add(read.GetBoolean(3), read.GetString(2));
        //                                }

        //                            }

        //                            gen.setItems(dict);

        //                            read.Close();
        //                        }

        //                        if (theListONLY)
        //                            list.Add(gen);
        //                        else
        //                            this.Add(gen);
        //                    }
        //                    else
        //                    {
        //                        var flex = new FLEXI_NOTE();

        //                        flex.setAuthor(account); // ACCOUNT
        //                        flex.setProjectName(proList.ElementAt(i)); // PROJECT
        //                        flex.setApplicationName(appList.ElementAt(i)); // APPLICATION
        //                        flex.setStartTime(startList.ElementAt(i)); // START TIME
        //                        flex.setEndTime(endList.ElementAt(i)); // END TIME
        //                        flex.setOutput(outputList.ElementAt(i)); // OUTPUT
        //                        flex.setType(typeList.ElementAt(i)); // TYPE
        //                        flex.setNotarised(notaries.ElementAt(i)); // Notaries

        //                        query.CommandText = "SELECT * FROM FLEXI_NOTE_LOG ORDER BY logID ASC, flexiNoteID ASC;";
        //                        read = query.ExecuteReader();

        //                        while (read.Read())
        //                        {
        //                            if (ids.ElementAt(i) == read.GetInt32(0))
        //                            {
        //                                if (FNCategoryList.Item1 == read.GetInt32(1))
        //                                    flex.setCategory(FNCategoryList.Item2);

        //                                if (mediumList.Item1 == read.GetInt32(2))
        //                                    flex.setMedium(mediumList.Item2);

        //                                if (formatList.Item1 == read.GetInt32(3))
        //                                    flex.setFormat(formatList.Item2);

        //                                flex.setBitRate(read.GetInt32(4));
        //                                flex.setLength(read.GetString(5));
        //                                flex.setCompleted(read.GetBoolean(6));
        //                                flex.setSource(read.GetString(7));
        //                                flex.setGameCategory(read.GetBoolean(8));

        //                                break;
        //                            }

        //                        }

        //                        read.Close();

        //                        if (theListONLY)
        //                            list.Add(flex);
        //                        else
        //                            this.Add(flex);
        //                    }

        //                }
        //                break; // case 4 SWITCH
        //        }
        //    }

        //    return list;
        //}




        // ---END MEMBER FUNCTIONS

        public string UpdateProfilePic(string emailAddress)
        {
            string filePath = "";

            var profilePicColumn = 1;

            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM ACCOUNT WHERE email = @emailAddress;";
            query.Parameters.AddWithValue("@emailAddress", emailAddress);

            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                filePath = read.GetString(profilePicColumn);
                break;
            }


            return filePath;
        }
    }
}

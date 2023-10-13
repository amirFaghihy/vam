namespace Aban.ViewModel
{
    public class BreadCrumb
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BreadCrumb()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        public string Title { get; set; }
        public string PageTitle { get; set; }

        public List<string[]> Links { get; set; }

    }

}
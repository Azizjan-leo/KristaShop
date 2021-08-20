namespace Module.App.Admin.Admin.Models {
    public class TopMenuDashboardViewModel {
        public int NewUsers { get; set; }
        public int NewOrders { get; set; }
        public int ClientActionsTotal => NewUsers + NewOrders;
        public int FeedbackTotal { get; set; }

        public TopMenuDashboardViewModel(int newUsers, int newOrders, int newFeedbacks)
        {
            NewUsers = newUsers;
            NewOrders = newOrders;
            FeedbackTotal = newFeedbacks;
        }
    }
}

using System.Collections.Generic;

namespace AffiliateCrawlers.Pages
{
    public interface ICrawlPage
    {
        List<string> Start(int numberOfItems);

        List<string> GetAllItems(int numberOfItems);
    }
}
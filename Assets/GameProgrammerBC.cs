//Contains contact information for a game programmer.
public class GameProgrammerBC : BusinessCard
{
    private string name = "MARLOW GREENAN";
    private ERole[] roles = { ERole.PROGRAMMING, ERole.PRODUCTION, ERole.DESIGN };
    private SLink[] findMeOn = 
          { new SLink("Email", "marlowgreenan@gmail.com"), 
            new SLink("Phone", "(217)-341-5341"),
            new SLink("GitHub", "https://github.com/IceyFox006"),
            new SLink("LinkedIn", "https://www.linkedin.com/in/marlow-greenan-7b7617334/")};
}

public struct SLink
{
    private string type;
    private string info;

    public SLink(string t, string i)
    {
        type = t;
        info = i;
    }
}

public enum ERole
{
    PRODUCTION,
    DESIGN,
    ART,
    PROGRAMMING,
    UIUX
}

public class BusinessCard
{

}
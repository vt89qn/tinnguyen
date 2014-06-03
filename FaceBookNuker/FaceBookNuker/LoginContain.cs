public class LoginContain
{
    public string UserName { set; get; }
    public string PassWord { set; get; }
    public LoginContain()
    {
        this.UserName = string.Empty;
        this.PassWord = string.Empty;
    }
    public LoginContain(string strUserName, string strPassWord)
    {
        this.UserName = strUserName;
        this.PassWord = strPassWord;
    }
}

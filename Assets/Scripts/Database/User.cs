public class User {
    public string username;
    public string email;
    public string gnome;
    public bool startGameStatus;

    public User(string username, string email) {
        this.username = username;
        this.email = email;
        this.gnome = "";
        this.startGameStatus = false;
    }
}


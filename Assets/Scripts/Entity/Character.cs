public class Charater {
    public int hp;
    private Observer observer;
    public Charater () {
        observer = new Observer ();
    }

    public virtual void onDeath () { }
}
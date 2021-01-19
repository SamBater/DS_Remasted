public class Weapon : Item
{
    public Damage ATK;
    public BaseStates bounusLv;
    public WpAtkMotionID wpAtkMotionID;
    public int spAtkMotionID;
    public int localMotionID1H;
    public int localMotionID2H;
    public override void LoadData(string[] col)
    {
       base.LoadData(col);
       itemType = ItemType.Weapon;
       wpAtkMotionID = (WpAtkMotionID) int.Parse(col[4]);
       spAtkMotionID = int.Parse(col[5]);
       localMotionID1H = int.Parse(col[6]);
       localMotionID2H = int.Parse(col[7]);
    }
}
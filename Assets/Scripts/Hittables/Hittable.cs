using UnityEngine;

[CreateAssetMenu(fileName = "New Hittable", menuName = "Hittable")] 
public class Hittable : ScriptableObject
{
    public int health;
    public AudioClip audioClip;
    public virtual void SetUp() { }
    public virtual void OnDead(GameController gc, Transform t) { }
    public virtual void OnMiss(GameController gc, Transform t) { }
}
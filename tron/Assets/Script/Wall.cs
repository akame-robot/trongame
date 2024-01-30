using UnityEngine;
using Photon.Pun;

public class Wall : MonoBehaviourPunCallbacks
{
    private GameObject owner;

    // Set the owner of the wall
    public void SetOwner(GameObject player)
    {
        owner = player;
    }

    // Check if the wall belongs to the given player
    public bool IsOwner(GameObject player)
    {
        return owner == player;
    }
}

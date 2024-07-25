using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string prompt;
    [SerializeField] private string conversation;
    [SerializeField] private bool showConversation;
    [SerializeField] private Collectible cost;
    [SerializeField] private Collectible prize;

    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetPrompt()
    {
        return prompt;
    }

    public string GetConversation()
    {
        return conversation;
    }

    public bool ShouldShowConversation()
    {
        return showConversation;
    }

    public Collectible GetCost()
    {
        return cost;
    }

    public Collectible GetPrize()
    {
        return prize;
    }

    public bool IsInteractableActive()
    {
        return active;
    }

    public void DeactivateInteractable()
    {
        active = false;
    }

}

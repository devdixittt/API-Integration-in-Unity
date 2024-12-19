using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Elements")]
    public Dropdown filterDropdown;
    public GameObject clientListContainer;
    public GameObject clientItemPrefab;
    public GameObject clientPopup;
    public Text popupName;
    public Text popupPoints;
    public Text popupAddress;

    private List<Client> allClients;

    private void OnEnable()
    {
        APIManager.DataFetched += PopulateClientList;
    }

    private void OnDisable()
    {
        APIManager.DataFetched -= PopulateClientList;
    }

    void Start()
    {
        // Check if APIManager is properly initialized
        APIManager apiManager = FindObjectOfType<APIManager>();
        if (apiManager == null)
        {
            Debug.LogError("APIManager is not found in the scene!");
            return;
        }

        // Fetch data from API
        apiManager.FetchClientData();

        // Ensure Dropdown is properly linked
        if (filterDropdown != null)
        {
            filterDropdown.onValueChanged.AddListener(FilterClients);
        }
        else
        {
            Debug.LogError("FilterDropdown is not assigned!");
        }
    }


    private void PopulateClientList(List<Client> clients)
    {
        allClients = clients;
        FilterClients(filterDropdown.value);
    }

    private void FilterClients(int filterIndex)
    {
        // Clear existing items.
        foreach (Transform child in clientListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Determine filter.
        List<Client> filteredClients = new List<Client>();
        switch (filterIndex)
        {
            case 0: // All
                filteredClients = allClients;
                break;
            case 1: // Managers
                filteredClients = allClients.FindAll(c => c.role == "Managers");
                break;
            case 2: // Non-Managers
                filteredClients = allClients.FindAll(c => c.role == "Non Managers");
                break;
        }

        // Populate list.
        foreach (Client client in filteredClients)
        {
            GameObject newItem = Instantiate(clientItemPrefab, clientListContainer.transform);
            newItem.GetComponentInChildren<Text>().text = $"{client.label} - {client.points}";

            // Set up click event.
            newItem.GetComponent<Button>().onClick.AddListener(() => ShowClientDetails(client));
        }
    }

    public void ShowClientDetails(Client client)
    {
        popupName.text = $"Name: {client.label}";
        popupPoints.text = $"Points: {client.points}";
        popupAddress.text = $"Address: {client.address}";

        FindObjectOfType<UIAnimator>().ShowPopup();
    }

    public void ClosePopup()
    {
        FindObjectOfType<UIAnimator>().HidePopup();
    }
}

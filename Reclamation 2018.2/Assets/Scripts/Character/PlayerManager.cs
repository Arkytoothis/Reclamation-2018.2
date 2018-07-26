using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : Singleton<PlayerManager>
{
    const int MaxParties = 10;

    public Transform playerSpawn;
    public GameObject partyPrefab;

    public int numPartiesUnlocked;
    public List<GameObject> parties;

    public PartyPanel partyPanel;

    public void Initialize()
    {
        parties = new List<GameObject>(MaxParties);
        numPartiesUnlocked = 1;

        PartyData party = new PartyData("Blue Party", Color.blue, 0, 3);

        party.pcs[0] = new PC(PcGenerator.Generate(0, "Imperial", "Soldier"));
        party.pcs[1] = new PC(PcGenerator.Generate(1, "Imperial", "Scout"));
        party.pcs[2] = new PC(PcGenerator.Generate(2, "Imperial", "Priest"));

        parties.Add(CreatePartyObject(this.transform, playerSpawn.position, party));

        partyPanel.Initialize();

        ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[0].pivot, party.pcs[0]);
        ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[1].pivot, party.pcs[1]);
        ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[2].pivot, party.pcs[2]);
    }

    public GameObject CreatePartyObject(Transform parent, Vector3 position, PartyData data)
    {
        float y = Terrain.activeTerrain.SampleHeight(new Vector3(position.x, 0, position.z));

        GameObject partyGO = Instantiate(partyPrefab);
        partyGO.name = data.name;
        partyGO.transform.position = new Vector3(position.x, y - 0.001f, position.z); 

        PartyController partyController = partyGO.GetComponent<PartyController>();

        GameObject pcGO = Instantiate(ModelManager.instance.GetPrefab(data.pcs[0]), new Vector3(position.x, y - 0.001f, position.z), Quaternion.identity);
        pcGO.name = data.name + " PC ";

        partyGO.transform.SetParent(pcGO.transform);

        partyController.SetPartyData(data);
        partyController.SetPcAnimator(pcGO.GetComponent<PcAnimator>());
        partyController.SetPcMotor(pcGO.GetComponent<WorldPcMotor>());

        Camera.main.transform.SetParent(pcGO.transform);
        CameraController cam = Camera.main.GetComponent<CameraController>();
        cam.target = partyGO.transform;

        return partyGO;
    }

    public void CreatePortraitModel(Transform parent, PC pc)
    {
        ModelManager.instance.SpawnCharacter(parent, pc);
    }
}
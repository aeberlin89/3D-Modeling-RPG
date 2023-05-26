using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    #region singleton

    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Equipment[] defaultItems;

    public SkinnedMeshRenderer targetMesh;

    Inventory inventory;
    //array keeping track of all of the currently equiped items
    Equipment[] currentEquipment;

    //create array of skinned mesh renderers that we're going to be spawning into scene
    SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();

    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Debug.Log("Equip Slot: " + newItem.equipSlot);

        //unequip old item and save in oldItem variable
        Equipment oldItem = Unequip(slotIndex);

        onEquipmentChanged?.Invoke(newItem, oldItem);

        SetEquipmentBlendShapes(newItem, 100);

        currentEquipment[slotIndex] = newItem;

        // now we need to instantiate the new equipment mesh
        // remember the new item (equipment) has a public mesh variable that allows us to
        // change the mesh in the editor via the scriptable object
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);

        // now set its parent object to the target mesh (player body mesh) so that
        // we can tell it how to deform. We want it to deform based on the bones of
        // the target mesh
        newMesh.transform.parent = targetMesh.transform;
        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        // now insert the new mesh into the currentMeshes array so we know it's equipped
        currentMeshes[slotIndex] = newMesh;


    }

    //unequip a certain item
    public Equipment Unequip (int slotIndex)
    {
        //we only want to do this if an item is equipped
        if(currentEquipment[slotIndex] != null)
        {
            //check that the current mesh is also not null
            if(currentMeshes[slotIndex] != null)
            {
                //destroy the mesh
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            //add the item to the inventory
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            //set to zero the weight of the blend shape of the areas that the
            //item used to cover. pass in the old item, the regions are handled
            //in the method
            SetEquipmentBlendShapes(oldItem, 0);

            //remove the item from the equipment array
            currentEquipment[slotIndex] = null;

            //trigger callback
            onEquipmentChanged?.Invoke(null, oldItem);

            return oldItem;
        }

        return null;
    }


    //unequip all items
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefaultItems();
    }


    //control the blendshapes in the body so that they do not intersect with armor meshes
    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        //loop through each region that the particular item covers
        foreach(EquipmentMeshRegion blendshape in item.coveredMeshRegions)
        {
            //set the blend shape weight. cast blendshape to an integer
            //this works because the enum is indexed in the same order as
            //the blend shapes on the targetMesh (the player's body SMR)
            targetMesh.SetBlendShapeWeight((int)blendshape, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach(Equipment item in defaultItems)
        {
            Equip(item);
        }
    }




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }


}




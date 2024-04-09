using System.Collections.Generic;
using UnityEngine;


public class garaScript : MonoBehaviour
{
    [Header("gamaobjects")]
    [SerializeField] Transform frente, trais;
    public GameObject isRuning;

    [Header("gerenciamento")]
    [SerializeField] float SpeedForSeconds;
    private float mSpeed = 1f;
    private float svspeed;
    [SerializeField]private float time = 0;
    [SerializeField]private bool discanso = false;

    [Header("list")]
     public List<Slot> inputintrustriSlot = new List<Slot>();
     public List<Slot> outputtrustriSlot = new List<Slot>();
     public List<Slot> allChestSlotinput = new List<Slot>();
     public List<Slot> allChestSlotoutput = new List<Slot>();
    public List<Slot> allInventorySlot = new List<Slot>();

    [Header("layer")]
    [SerializeField] LayerMask update;

    //scrits
    [SerializeField]IndustrialScripts[] IndustrialScripts = new IndustrialScripts[2];
    [SerializeField] Chest[] chest = new Chest[2];
    [SerializeField] Belt[] belt = new Belt[2];
    [SerializeField] missaoScripter missao;

    //componetes
    Animator animator;

    //instancia e update
    private void Start()
    {
        
        animator = GetComponent<Animator>();
        animator.speed = 0;
        svspeed = mSpeed / SpeedForSeconds;
        time = svspeed;
        updatelocal();
        OnDestroy();
    }

    private void OnDestroy()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f), Vector2.down, 0.5F, update);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F, update);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F, update);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F, update);
        if (down.collider)
        {
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
            if (down.collider.CompareTag("belt"))
                down.collider.GetComponent<Belt>().updatelocal();
            if (down.collider.CompareTag("spliter"))
                down.collider.GetComponent<Spliter>().updatelocal();
            if (down.collider.CompareTag("Tunio"))
                down.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (lesft.collider)
        {
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
            if (lesft.collider.CompareTag("belt"))
                lesft.collider.GetComponent<Belt>().updatelocal();
            if (lesft.collider.CompareTag("spliter"))
                lesft.collider.GetComponent<Spliter>().updatelocal();
            if (lesft.collider.CompareTag("Tunio"))
                lesft.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (up.collider)
        {
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
            if (up.collider.CompareTag("belt"))
                up.collider.GetComponent<Belt>().updatelocal();
            if (up.collider.CompareTag("spliter"))
                up.collider.GetComponent<Spliter>().updatelocal();
            if (up.collider.CompareTag("Tunio"))
                up.collider.GetComponent<tunioScript>().updatelocal();
        }
        if (right.collider)
        {
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
            if (right.collider.CompareTag("belt"))
                right.collider.GetComponent<Belt>().updatelocal();
            if (right.collider.CompareTag("spliter"))
                right.collider.GetComponent<Spliter>().updatelocal();
            if (right.collider.CompareTag("Tunio"))
                right.collider.GetComponent<tunioScript>().updatelocal();
        }
    }
    public void updatelocal()
    {
        Invoke("updateData", 0.2f);
    }
    void updateData()
    {
        IndustrialScripts[1] = null;
        chest[1] = null;
        belt[1] = null;
        IndustrialScripts[0] = null;
        chest[0] = null;
        belt[0] = null;
        missao = null;
        
        RaycastHit2D m_HitDetect = Physics2D.Raycast(frente.position, Direction(frente), 0.5F);
        if (m_HitDetect)
        {

            IndustrialScripts[0] = m_HitDetect.collider.GetComponent<IndustrialScripts>();
            chest[0] = m_HitDetect.collider.GetComponent<Chest>();
            belt[0] = m_HitDetect.collider.GetComponent<Belt>();
            missao = m_HitDetect.collider.GetComponent<missaoScripter>();

            if (IndustrialScripts[0] != null)
                inputintrustriSlot = IndustrialScripts[0].inputintrustriSlot;
            if (chest[0] != null) {
                allChestSlotinput = chest[0].allChestSlot;
                allInventorySlot.AddRange(allChestSlotinput);
            }
            if (missao != null)
            {
                inputintrustriSlot = missao.inputintrustriSlot;
            }

        }
        RaycastHit2D segund = Physics2D.Raycast(trais.position, Direction(trais), 0.5F);
        if (segund)
        {
            IndustrialScripts[1] = segund.collider.GetComponent<IndustrialScripts>();
            chest[1] = segund.collider.GetComponent<Chest>();
            belt[1] = segund.collider.GetComponent<Belt>();

            if (IndustrialScripts[1] != null)
                outputtrustriSlot = IndustrialScripts[1].outputtrustriSlot;
            else
                outputtrustriSlot.Clear();
            if (chest[1] != null)  
                allChestSlotoutput = chest[1].allChestSlot;          
            else
                allChestSlotoutput.Clear();
        }
        //Invoke("updatelater", 0.2f);
    }
    void updatelater()
    {
        //seção de limpamento
        if (missao == null) inputintrustriSlot.Clear();
        if (IndustrialScripts[1] == null) outputtrustriSlot.Clear();
        if (IndustrialScripts[0] == null) inputintrustriSlot.Clear();
        if (chest[1] == null) allChestSlotoutput.Clear();
        if (chest[0] == null) allChestSlotinput.Clear();
        if (chest[0] != null)
        {
            allInventorySlot.AddRange(chest[0].allChestSlot);
        }
        else
        {
            allInventorySlot.Clear();
        }

    }
    private Vector2 Direction(Transform lateralDeSaida)
    {
        return (lateralDeSaida.position - transform.position).normalized;
    }

    private void Update()
    {
        if (IndustrialScripts[0] != null || chest[0] != null || belt[0] != null &&
            IndustrialScripts[1] != null || chest[1] != null || belt[1] != null)
        {          
            timeandtetection();
        }

    }

    void timeandtetection()
    {
        if (!discanso)
        {
            if (isRuning == null)
            {
                animator.speed = 0;
                if (IndustrialScripts[0] != null)
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (holdItem.MaxQuabttity > holdItem.currentQuantity) break;
                        if (i + 1 == inputintrustriSlot.Count) return;
                    }
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                Item holdItem = inputintrustriSlot[i].getItem();
                                if (belt[1].item != null) {
                                    if (holdItem == null || belt[1].item.GetComponent<Item>().ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        isRuning = belt[1].item;
                                        belt[1].item = null;
                                        break;
                                    }
                                }
                            }
                        }//
                    }
                    else if (chest[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = inputintrustriSlot[i].getItem();

                                for (int o = allChestSlotoutput.Count; o > 0 ; o--)
                                {
                                    Item holdItemout = allChestSlotoutput[o-1].getItem();
                                    
                                    if (holdItemout != null && holdItem != null && holdItemout.ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.SetActive(true);


                                        allChestSlotoutput[o - 1].UpdateData();

                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        if (holdItemout.currentQuantity <= 0)
                                        {
                                            allChestSlotoutput[o-1].SetItem(null);
                                            allChestSlotoutput[o-1].UpdateData();
                                        }                                      
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (IndustrialScripts[1])
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = inputintrustriSlot[i].getItem();

                                for (int o = 0; o < outputtrustriSlot.Count; o++)
                                {
                                    Item holdItemout = outputtrustriSlot[o].getItem();
                                    if (holdItemout.ID == holdItem.ID && holdItemout.currentQuantity > 0 && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        isRuning.SetActive(true);
                                        outputtrustriSlot[o-1].UpdateData();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                else if (chest[0] != null)
                {
                    for (int i = 0; i < allChestSlotinput.Count; i++)
                    {
                        Item holdItem = allChestSlotinput[i].getItem();
                        if (holdItem == null) break;
                        if (holdItem.MaxQuabttity > holdItem.currentQuantity) break;
                        if (i + 1 == allChestSlotinput.Count) return;
                    }
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < allChestSlotinput.Count; i++)
                            {
                                Item holdItem = allChestSlotinput[i].getItem();
                                if (holdItem == null || belt[1].item.GetComponent<Item>().ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                {
                                    isRuning = belt[1].item;
                                    belt[1].item = null;
                                    break;
                                }
                            }
                        }
                    }
                    else if (chest[1])
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < allChestSlotoutput.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = allChestSlotoutput[i].getItem();

                                for (int o = allChestSlotinput.Count; o > 0; o--)
                                {

                                    Item holdItemout = allChestSlotinput[o - 1].getItem();

                                    if (holdItem != null && holdItemout == null || holdItem != null && holdItemout.ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        holdItem.currentQuantity--;
                                        allChestSlotoutput[i].UpdateData();
                                        if (holdItem.currentQuantity == 0)
                                        {
                                            allChestSlotoutput[i].SetItem(null);
                                        }

                                        isRuning = Instantiate(holdItem.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItem.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        isRuning.SetActive(true);
                                        allChestSlotoutput[i].UpdateData();                                        
                                        allChestSlotoutput[i].UpdateData();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (IndustrialScripts[1])
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < allChestSlotinput.Count; i++)
                            {
                                if (isRuning != null) break;
                                Item holdItem = allChestSlotinput[i].getItem();

                                for (int o = 0; o < outputtrustriSlot.Count; o++)
                                {
                                    Item holdItemout = outputtrustriSlot[o].getItem();
                                    if (holdItem == null && holdItemout != null && holdItemout.currentQuantity > 0 || holdItemout != null && holdItemout.ID == holdItem.ID && holdItemout.currentQuantity > 0 && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        isRuning.SetActive(true);
                                        outputtrustriSlot[o - 1].UpdateData();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                else if (belt[0] != null)
                {
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            if (belt[1].item != null)
                            {
                                isRuning = belt[1].item;
                                belt[1].item = null;
                            }
                        }
                    }
                    else if (chest[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = allChestSlotoutput.Count; i > 0; i--)
                            { 
                                Item holdItem = allChestSlotoutput[i - 1].getItem();
                                if (holdItem != null)
                                {
                                    holdItem.currentQuantity--;
                                    isRuning = Instantiate(holdItem.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItem.gameObject.transform.rotation);
                                    isRuning.GetComponent<Item>().currentQuantity = 1;
                                    isRuning.SetActive(true);
                                    allChestSlotoutput[i - 1].UpdateData();
                                    if (holdItem.currentQuantity <= 0)
                                    {
                                        allChestSlotoutput[i - 1].SetItem(null);
                                    }
                                    allChestSlotoutput[i - 1].UpdateData();
                                    break;
                                }

                            }
                        }
                    }
                    else if (IndustrialScripts[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int o = 0; o < outputtrustriSlot.Count; o++)
                            {
                                Item holdItemout = outputtrustriSlot[o].getItem();
                                Debug.Log(holdItemout.currentQuantity);
                                if (holdItemout.currentQuantity > 0)
                                {
                                    holdItemout.currentQuantity--;
                                    isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                    isRuning.GetComponent<Item>().currentQuantity = 1;
                                    isRuning.SetActive(true);
                                    outputtrustriSlot[o - 1].UpdateData();
                                    break;
                                }
                            }
                        }
                    }
                }
                else if(missao != null)
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (holdItem == null) return;
                        if (holdItem.MaxQuabttity > holdItem.currentQuantity) break;
                        if (i + 1 == inputintrustriSlot.Count) return;
                    }
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                Item holdItem = inputintrustriSlot[i].getItem();
                                if (belt[1].item != null)
                                {
                                    if (holdItem == null || belt[1].item.GetComponent<Item>().ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        isRuning = belt[1].item;
                                        belt[1].item = null;
                                        break;
                                    }
                                }
                            }
                        }//
                    }
                    else if (chest[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = inputintrustriSlot[i].getItem();

                                for (int o = allChestSlotoutput.Count; o > 0; o--)
                                {
                                    Item holdItemout = allChestSlotoutput[o - 1].getItem();

                                    if (holdItemout != null && holdItem != null && holdItemout.ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.SetActive(true);

                                        allChestSlotoutput[o - 1].UpdateData();

                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        if (holdItemout.currentQuantity <= 0)
                                        {
                                            allChestSlotoutput[o - 1].SetItem(null);
                                            allChestSlotoutput[o - 1].UpdateData();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (IndustrialScripts[1])
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = inputintrustriSlot[i].getItem();

                                for (int o = 0; o < outputtrustriSlot.Count; o++)
                                {
                                    Item holdItemout = outputtrustriSlot[o].getItem();
                                    if (holdItemout.ID == holdItem.ID && holdItemout.currentQuantity > 0 && holdItem.MaxQuabttity > holdItem.currentQuantity)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        isRuning.SetActive(true);
                                        outputtrustriSlot[o - 1].UpdateData();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (isRuning == null) return;
            }

            isRuning.GetComponent<Collider2D>().enabled = true;

            if (svspeed / 2 >= time)
            {
                if (!isRuning.gameObject)
                {
                    isRuning = null;
                    return;
                }
                bool tama = false;
                if (IndustrialScripts[0])
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {                       
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (isRuning.GetComponent<Item>().ID != holdItem.ID || holdItem.currentQuantity >= holdItem.MaxQuabttity)
                        {
                           
                        }
                        else
                        {
                            tama = true;
                            break;
                        }
                    }
                    if (!tama)
                    {
                        animator.speed = 0;
                        return;
                    }
                }
                else if (chest[0] != null)
                {
                    for (int i = 0; i < allChestSlotinput.Count; i++)
                    {
                        Item holdItemchest = allChestSlotinput[i].getItem();
                        if (holdItemchest == null || holdItemchest.currentQuantity < holdItemchest.MaxQuabttity)
                        {
                            tama = true;
                            break;
                        }
                    }
                    if (!tama)
                    {
                        animator.speed = 0;
                        return;
                    }
                }
                else if (belt[0] != null)
                {
                    if (belt[0].item != null)
                    {
                        animator.speed = 0;
                        return;
                    }

                }

            }
            time -= Time.deltaTime;
            animator.speed = 1;         
            //tem um bag
            if (time <= 0)
            {
                discanso = true;
                if (IndustrialScripts[0] != null)
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (isRuning.GetComponent<Item>().ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                        {
                            holdItem.currentQuantity++;
                            inputintrustriSlot[i].UpdateData();
                            Destroy(isRuning);
                            isRuning = null;
                            time = svspeed;
                            return;
                        }
                    }
                }
                else if (chest[0] != null)
                {
                    if (isRuning != null)
                    {
                        if (chest[0].chestInstantiatedParent.activeSelf == false) 
                        {
                            chest[0].chestInstantiatedParent.SetActive(true);
                            addItemInventory(isRuning.GetComponent<Item>());
                            chest[0].chestInstantiatedParent.SetActive(false);
                        }
                        else
                        {
                            addItemInventory(isRuning.GetComponent<Item>());
                        }
                    }
                    time = svspeed;
                    isRuning = null;
                }
                else if (belt[0] != null)
                {
                    belt[0].item = isRuning;
                    isRuning = null;
                    time = svspeed;
                    return;
                }
                else if(missao != null)
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (isRuning.GetComponent<Item>().ID == holdItem.ID && holdItem.MaxQuabttity > holdItem.currentQuantity)
                        {
                            holdItem.currentQuantity++;
                            inputintrustriSlot[i].UpdateData();
                            Destroy(isRuning);
                            isRuning = null;
                            time = svspeed;
                            return;
                        }
                    }
                }
            }
            else
            {
                var distanceDelt = SpeedForSeconds*2 * Time.deltaTime;
                isRuning.transform.position = Vector3.MoveTowards(isRuning.transform.position,
                    frente.transform.position + (new Vector3(Direction(frente).x, Direction(frente).y, 0) * 0.5f),
                    distanceDelt);
            }
        }
        else
        {
            animator.speed = 1;
            time -= Time.deltaTime;
            if(time <= 0)
            {
                discanso = false;
                time = svspeed;
            }
        }
    }

    private void addItemInventory(Item itemToAdd, int overideIndex = -1)
    {
        if (overideIndex != -1)
        {
            if (overideIndex > allInventorySlot.Count) return;
            allInventorySlot[overideIndex].SetItem(itemToAdd);
            itemToAdd.gameObject.SetActive(false);
            allInventorySlot[overideIndex].UpdateData();
            return;
        }

        int leftoverQuantity = itemToAdd.currentQuantity;
        Slot openSholt = null;

        for (int i = 0; i < allInventorySlot.Count; i++)
        {
            Item heldItem = allInventorySlot[i].getItem();
            if (heldItem != null && itemToAdd.ID == heldItem.ID)
            {
                int freeSpaceInSlot = heldItem.MaxQuabttity - heldItem.currentQuantity;
                if (freeSpaceInSlot >= leftoverQuantity)
                {
                    heldItem.currentQuantity += leftoverQuantity;
                    Destroy(itemToAdd.gameObject);
                    allInventorySlot[i].UpdateData();
                    return;
                }
                else// DD as much as we can to the currest Slot
                {
                    heldItem.currentQuantity = heldItem.MaxQuabttity;
                    leftoverQuantity -= freeSpaceInSlot;
                }
            }
            else if (heldItem == null)
            {
                if (!openSholt)
                    openSholt = allInventorySlot[i];
            }

            allInventorySlot[i].UpdateData();
        }

        if (leftoverQuantity > 0 && openSholt)
        {
            openSholt.SetItem(itemToAdd);
            itemToAdd.currentQuantity = leftoverQuantity;
            itemToAdd.gameObject.SetActive(false);
        }
        else
        {
            itemToAdd.currentQuantity = leftoverQuantity;
        }
    }

}


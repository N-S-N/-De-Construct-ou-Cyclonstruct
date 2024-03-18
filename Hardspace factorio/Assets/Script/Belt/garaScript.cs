using System.Collections.Generic;
using Unity.VisualScripting;
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
    [HideInInspector] public List<Slot> inputintrustriSlot = new List<Slot>();
    [HideInInspector] public List<Slot> outputtrustriSlot = new List<Slot>();
    [HideInInspector] public List<Slot> allChestSlotinput = new List<Slot>();
    [HideInInspector] public List<Slot> allChestSlotoutput = new List<Slot>();

    //scrits
    IndustrialScripts[] IndustrialScripts = new IndustrialScripts[2];
    Chest[] chest = new Chest[2];
    Belt[] belt = new Belt[2];

    //componetes
    Animator animator;

    //instancia e update
    private void Start()
    {
        
        animator = GetComponent<Animator>();
        svspeed = mSpeed / SpeedForSeconds;
        time = svspeed;
        updatelocal();
        OnDestroy();
    }

    private void OnDestroy()
    {
        RaycastHit2D down = Physics2D.Raycast(transform.position + new Vector3(0,-0.5f), Vector2.down, 0.5F);
        RaycastHit2D lesft = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.left, 0.5F);
        RaycastHit2D up = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.up, 0.5F);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.right, 0.5F);
        if (down.collider)
        {
            if (down.collider.CompareTag("garra"))
                down.collider.GetComponent<garaScript>().updatelocal();
            if (down.collider.CompareTag("belt"))
                down.collider.GetComponent<Belt>().updatelocal();
            if (down.collider.CompareTag("spliter"))
                down.collider.GetComponent<Spliter>().updatelocal();
        }
        if (lesft.collider)
        {
            if (lesft.collider.CompareTag("garra"))
                lesft.collider.GetComponent<garaScript>().updatelocal();
            if (lesft.collider.CompareTag("belt"))
                lesft.collider.GetComponent<Belt>().updatelocal();
            if (lesft.collider.CompareTag("spliter"))
                lesft.collider.GetComponent<Spliter>().updatelocal();
        }
        if (up.collider)
        {
            if (up.collider.CompareTag("garra"))
                up.collider.GetComponent<garaScript>().updatelocal();
            if (up.collider.CompareTag("belt"))
                up.collider.GetComponent<Belt>().updatelocal();
            if (up.collider.CompareTag("spliter"))
                up.collider.GetComponent<Spliter>().updatelocal();
        }
        if (right.collider)
        {
            if (right.collider.CompareTag("garra"))
                right.collider.GetComponent<garaScript>().updatelocal();
            if (right.collider.CompareTag("belt"))
                right.collider.GetComponent<Belt>().updatelocal();
            if (right.collider.CompareTag("spliter"))
                right.collider.GetComponent<Spliter>().updatelocal();
        }
    }
    public void updatelocal()
    {
        RaycastHit2D m_HitDetect = Physics2D.Raycast(frente.position, Direction(frente), 0.5F);
        if (m_HitDetect)
        {
            IndustrialScripts[0] = m_HitDetect.collider.GetComponent<IndustrialScripts>();
            chest[0] = m_HitDetect.collider.GetComponent<Chest>();
            belt[0] = m_HitDetect.collider.GetComponent<Belt>();

            if (IndustrialScripts[0] != null)
                inputintrustriSlot = IndustrialScripts[0].inputintrustriSlot;
            else
                inputintrustriSlot.Clear();
            if (chest[0] != null)
                allChestSlotinput = chest[0].allChestSlot;
            else
                allChestSlotinput.Clear();
            
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
            if(discanso && animator.speed > 0 || !discanso && animator.speed < 0)
                animator.speed *= -1;
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
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                Item holdItem = inputintrustriSlot[i].getItem();
                                if (belt[1].item.GetComponent<Item>().ID == holdItem.ID)
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
                            for (int i = 0; i < inputintrustriSlot.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = inputintrustriSlot[i].getItem();

                                for (int o = allChestSlotoutput.Count; o > 0 ; o--)
                                {

                                    Item holdItemout = allChestSlotoutput[o-1].getItem();
                                    
                                    if (holdItemout != null && inputintrustriSlot != null && holdItemout.ID == holdItem.ID)
                                    {
                                        
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.SetActive(true);


                                        allChestSlotoutput[o - 1].UpdateData();

                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        if (holdItemout.currentQuantity <= 0)
                                        {
                                            Debug.Log("aa");
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
                                    if (holdItemout.ID == holdItem.ID && holdItemout.currentQuantity > 0)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
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
                    if (belt[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = 0; i < allChestSlotinput.Count; i++)
                            {
                                Item holdItem = allChestSlotinput[i].getItem();
                                if (holdItem == null || belt[1].item.GetComponent<Item>().ID == holdItem.ID)
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
                            for (int i = 0; i < allChestSlotinput.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = allChestSlotinput[i].getItem();

                                for (int o = allChestSlotoutput.Count; o > 0; o--)
                                {

                                    Item holdItemout = allChestSlotoutput[o - 1].getItem();

                                    if (holdItemout == null || holdItemout.ID == holdItem.ID)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
                                        allChestSlotoutput[o - 1].UpdateData();
                                        if (holdItemout.currentQuantity <= 0)
                                        {
                                            allChestSlotoutput[o - 1].SetItem(null);
                                        }
                                        allChestSlotoutput[o-1].UpdateData();
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
                            for (int i = 0; i < allChestSlotoutput.Count; i++)
                            {
                                if (isRuning != null) break;

                                Item holdItem = allChestSlotoutput[i].getItem();

                                for (int o = 0; o < outputtrustriSlot.Count; o++)
                                {
                                    Item holdItemout = outputtrustriSlot[o].getItem();
                                    if (holdItem == null && holdItemout.currentQuantity > 0 || holdItemout.ID == holdItem.ID && holdItemout.currentQuantity > 0)
                                    {
                                        holdItemout.currentQuantity--;
                                        isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                        isRuning.GetComponent<Item>().currentQuantity = 1;
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
                    if (isRuning == null)
                    {
                        if (belt[1].item != null)
                        {
                            isRuning = belt[1].item;
                            belt[1].item = null;
                        }
                    }
                    else if (chest[1] != null)
                    {
                        if (isRuning == null)
                        {
                            for (int i = allChestSlotoutput.Count; i > 0; i--)
                            {
                                Item holdItem = allChestSlotinput[i-1].getItem();
                                if (holdItem != null)
                                {
                                    holdItem.currentQuantity--;
                                    isRuning = Instantiate(holdItem.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItem.gameObject.transform.rotation);
                                    isRuning.GetComponent<Item>().currentQuantity = 1;
                                    allChestSlotoutput[i - 1].UpdateData();
                                    if (holdItem.currentQuantity <= 0)
                                    {
                                        allChestSlotoutput[i-1].SetItem(null);
                                    }
                                    allChestSlotoutput[i-1].UpdateData();
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
                                if (holdItemout.currentQuantity > 0)
                                {
                                    holdItemout.currentQuantity--;
                                    isRuning = Instantiate(holdItemout.gameObject, trais.position + (new Vector3(Direction(trais).x, Direction(trais).y, 0) * 0.5f), holdItemout.gameObject.transform.rotation);
                                    isRuning.GetComponent<Item>().currentQuantity = 1;
                                    outputtrustriSlot[o - 1].UpdateData();
                                    break;
                                }
                            }
                        }
                    }
                }
                if (isRuning == null) return;
            }

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
            if (time <= 0)
            {
                discanso = true;
                if (IndustrialScripts[0] != null)
                {
                    for (int i = 0; i < inputintrustriSlot.Count; i++)
                    {
                        Item holdItem = inputintrustriSlot[i].getItem();
                        if (isRuning.GetComponent<Item>().ID == holdItem.ID)
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
                    for (int i = 0; i < allChestSlotinput.Count; i++)
                    {
                        Item holdItem = allChestSlotinput[i].getItem();
                        if (isRuning.GetComponent<Item>().ID == holdItem.ID)
                        {
                            holdItem.currentQuantity++;
                            allChestSlotinput[i].UpdateData();
                            Destroy(isRuning);
                            isRuning = null;
                            time = svspeed;
                            return;
                        }
                    }
                    for (int i = 0; i < allChestSlotinput.Count; i++)
                    {
                        Item holdItem = allChestSlotinput[i].getItem();
                        if (holdItem == null)
                        {
                            allChestSlotinput[i].SetItem(isRuning.GetComponent<Item>());
                            allChestSlotinput[i].UpdateData();
                            Destroy(isRuning);
                            isRuning = null;
                            time = svspeed;
                            return;
                        }
                    }
                }
                else if (belt[0] != null)
                {
                    belt[0].item = isRuning;
                    isRuning = null;
                    time = svspeed;
                    return;
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
}


using UnityEngine;
using System.Collections;

public class FruitAnim : MonoBehaviour {
    public GameObject AnimObject;
    public GameObject AnimEffectObject;

    public Animator anim;
    public Animator animEffect;
    public int col; 
    public int row;


    public void setplayAnimDestroy()
    {
        if (anim ==null)
            anim = AnimObject.GetComponent<Animator>();
        anim.Play("FruitAnimDestroy");
    }
    public void setplayAnimDestroy2()
    {
        if (anim == null)
            anim = AnimObject.GetComponent<Animator>();
        anim.Play("FruitAnimDestroy2");
    }
    public void setplayEffectScore()
    {
        if (animEffect == null)
            animEffect = AnimEffectObject.GetComponent<Animator>();
        animEffect.Play("FruitAnimScore");
    }
    public void setplayEffectScoreX2()
    {
        if (animEffect == null)
            animEffect = AnimEffectObject.GetComponent<Animator>();
        animEffect.Play("FruitAnimScoreX2");
    }
    public void setIndexLink2Object(int index)//duong oi giua 2 thang
    {
		if(anim == null)
     	   anim = AnimObject.GetComponent<Animator>();
        anim.Play("FruitAnimConnect" + (index).ToString());
    }

	public void setAnimObjectToNONE()//duong oi giua 2 thang
	{
		if(anim == null)
			anim = AnimObject.GetComponent<Animator>();
		anim.Play("FruitAnimNONE");
	}
    public void setAnimThunder()//duong oi giua 2 thang
    {
        if (anim == null)
            anim = AnimObject.GetComponent<Animator>();
        anim.Play("FruitAnimThunder");
    }
    
}

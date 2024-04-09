using UnityEngine;
using UnityEditor;
using System.IO;

public class ReadWriteInfo : MonoBehaviour
{
    public static ReadWriteInfo instance;
    private void Start()
    {
        instance = this;
        ReadString();
    }
    public void WriteString(int amounttocollect)
    {
        string path = "Resources/CollectAmount.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Amount "+ amounttocollect + "      Time  "+ System.DateTime.Now);
        writer.Close();
        //Re-import the file to update the reference in the editor
    //    AssetDatabase.ImportAsset(path);
      //  TextAsset asset = Resources.Load("test");
        //Print the text from the file
      //  Debug.Log(asset.text);
    }
   
    public void ReadString()
    {
        string path = "Resources/DepositAmount.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

       //  Debug.Log(reader.ReadLine());  // It will only read onetime so if not commented, in the next line amount value is null

        string amount = reader.ReadLine();
        Debug.Log("mmm " + amount);

        if (amount == "")
        {
            Debug.Log("Null ");
            return;
        }
       // LobbyManager.instance.UpdateAmount(amount);
        reader.Close();

        StreamWriter writer = new StreamWriter(path, false); // If true it will add next line but with false it will replace all text
        writer.WriteLine("");
        writer.Close();
    }
    
}
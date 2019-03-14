using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReceiptWriter : MonoBehaviour {
    public static ReceiptWriter instance;

    private string fileName = "receipt.txt";

    private List<string> receiptEntries = new List<string>();
    private int total = 0;

    private void Awake() {
        instance = this;
    }

    public void AddToReceipt(string objectName, int objectPrice) {
        receiptEntries.Add(objectName + " $" + objectPrice.ToString());
        total += objectPrice;

        StreamWriter sw = new StreamWriter(Application.dataPath + "/../" + fileName, false);

        // Write each line in the reciept.
        sw.WriteLine("Receipt for Circle0");

        foreach (string receiptEntry in receiptEntries) {
            sw.WriteLine(receiptEntry);
        }

        sw.WriteLine("");
        sw.WriteLine("Your total: $" + total.ToString());
        sw.WriteLine("");
        sw.WriteLine("Thank you for your purchase.");
        sw.WriteLine("Please pay in cash.");
        sw.WriteLine("");
        sw.WriteLine("Visit us again at https://yesyes.itch.io");

        sw.Close();
    }
}

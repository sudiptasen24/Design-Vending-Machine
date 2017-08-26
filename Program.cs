using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication22
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine obj = new VendingMachine();
            obj.Start();
        }
    }

    public class VendingMachine
    {
        public VendingMachineState currentState = null;

        public VendingMachine()
        {
           
            currentState = new Selectproduct(this);
        }
        public void Start()
        {
            Console.WriteLine("Machine Starting Up");
            ItemRepository.ItemInstance.FillItems();
            while (true)
            {
                currentState.DisplayNextScreen();
            }

        }
    }
    public enum Item
    {
        Coke,
        Pepsi,
        Fanta
    }

    public class ItemRepository
    {
        Dictionary<Item, int> items = new Dictionary<Item, int>();
        Dictionary<Item, int> itemsprice = new Dictionary<Item, int>();

        private static ItemRepository itemrepo;
        private ItemRepository()
        {

        }

        public static ItemRepository ItemInstance
        {
            get
            {
                if (itemrepo == null)
                {
                    itemrepo = new ItemRepository();

                }
                return itemrepo;
            }
        }
        public Dictionary<Item, int> ItemCollection
        {
            get { return items; }
        }

        public Dictionary<Item, int> ItemPrice
        {

            get { return itemsprice; }
        }
        public void FillItems()
        {
            items.Add(Item.Coke, 2);

            items.Add(Item.Pepsi, 1);

            items.Add(Item.Fanta, 3);

            itemsprice.Add(Item.Coke, 20);
            itemsprice.Add(Item.Pepsi, 10);
            itemsprice.Add(Item.Fanta, 30);

        }
    }
    }

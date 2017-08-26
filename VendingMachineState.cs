using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication22
{
    public abstract class VendingMachineState
    {
        private VendingMachine vendor;

        public int AmountPaid
        {
            get; set;
        }
        public VendingMachine Vendor
        {
            get { return vendor; }
            set { vendor = value; }
        }
        public Item selecteditem
        {
            get; set;
        }
        public int itemsprice
        {
            get
            {
                var itemsprice = ItemRepository.ItemInstance.ItemPrice;
                return itemsprice[selecteditem];
            }
        }
        public abstract void DisplayNextScreen();
    }

    public class Selectproduct : VendingMachineState
    {
      public Selectproduct(VendingMachine obj)
        {
            Vendor = obj;
            AmountPaid = 0;
          
        }
        public override void DisplayNextScreen()
        {
            Console.WriteLine("select item : Coke,Pepsi,Fanta");
            string agr = Console.ReadLine();
            selecteditem = (Item)Enum.Parse(typeof(Item), agr);
            if (ItemRepository.ItemInstance.ItemCollection[selecteditem] > 0)
            {
                UpdateState();

            }
            else
            {
                Console.WriteLine("item not available");
            }
        }
        private void UpdateState()
        {
            Vendor.currentState = new GetPrice(this);
        }
    }
    public class GetPrice : VendingMachineState
    {
        public GetPrice(VendingMachineState obj)
        {
            Vendor = obj.Vendor;
            selecteditem = obj.selecteditem;

        }
        public override void DisplayNextScreen()
        {
            Console.WriteLine("Pay amount : " + itemsprice);
          
            UpdateState();
        }
        private void UpdateState()
        {
            Vendor.currentState = new PayMoney(this);
        }
    }
    public class PayMoney : VendingMachineState
    {
        public PayMoney(VendingMachineState obj)
        {
            Vendor = obj.Vendor;
            AmountPaid = obj.AmountPaid;
            selecteditem = obj.selecteditem;
        }
        public override void DisplayNextScreen()
        {
            Console.WriteLine("Enter amount you want to pay : ");
            string agr = Console.ReadLine();
            var x = Convert.ToInt32(agr);
            AmountPaid = AmountPaid + x;
            if (itemsprice == AmountPaid)
            {
                UpdateState(true);

            }
            else
            {
                UpdateState(false);
               
            }
        }
        private void UpdateState(bool complete)
        {
            if (complete)
                Vendor.currentState = new GetItem(this);
            else
            {
                if(AmountPaid < itemsprice)
                    Vendor.currentState = new PayRemaining(this);
                else
                    Vendor.currentState = new PayRefund(this);
            }

        }
    }
    public class PayRefund : VendingMachineState
    {
        public PayRefund(VendingMachineState obj)
        {
            Vendor = obj.Vendor;
            AmountPaid = obj.AmountPaid;
            selecteditem = obj.selecteditem;
        }
        public override void DisplayNextScreen()
        {
            Console.WriteLine("Accept refund amount  : " + (AmountPaid - itemsprice).ToString());
           
            UpdateState();

        }
        private void UpdateState()
        {
          
           Vendor.currentState = new GetItem(this);
         
        }
    }
    public class PayRemaining : VendingMachineState
    {
        public PayRemaining(VendingMachineState obj)
        {
            Vendor = obj.Vendor;
            AmountPaid = obj.AmountPaid;
            selecteditem = obj.selecteditem;
        }
        public override void DisplayNextScreen()
        {
            Console.WriteLine("Pay remaining amount  : "+(itemsprice - AmountPaid).ToString());
            Console.WriteLine("Enter amount you want to pay : ");
            string agr = Console.ReadLine();
            var x = Convert.ToInt32(agr);
            AmountPaid = AmountPaid + x;
            if (itemsprice == AmountPaid)
            {
                UpdateState(true);

            }
            else
            {
                UpdateState(false);

            }
        }
        private void UpdateState(bool complete)
        {
            if (complete)
                Vendor.currentState = new GetItem(this);
            else
            {
                if (AmountPaid < itemsprice)
                    Vendor.currentState = new PayRemaining(this);
                else
                    Vendor.currentState = new PayRefund(this);
            }

        }
    }
    public class GetItem : VendingMachineState
    {
    public GetItem(VendingMachineState obj)
    {
        Vendor = obj.Vendor;
            selecteditem = obj.selecteditem;

        }
    public override void DisplayNextScreen()
        {
            Console.WriteLine("Your transaction is successful for Item "+selecteditem.ToString());
            UpdateState();


        }
        private void UpdateState()
        {
            ItemRepository.ItemInstance.ItemCollection[selecteditem] = ItemRepository.ItemInstance.ItemCollection[selecteditem] - 1;
            Vendor.currentState = new Selectproduct(this.Vendor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    public partial class Menu
    {
        void updateOptions()
        {
            Console.WriteLine("What would you like to update?\n" +
                "1: Modify fields of an item\n" +
                "2: Update actions of the Program\n");

            int choice = 0;
            Int32.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 1:
                    modifyFields();
                    break;
                case 2:
                    updateActions();
                    break;
                default:
                    Console.WriteLine("Did not receive proper command!\n");
                    break;
            }






        }

        void modifyFields()
        {
            Console.WriteLine("What Item would you like to modify?\n" +
                "1: Drone\n" +
                "2: Customer\n" +
                "3: Station\n");
            int choice = 0;
            Int32.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 1: modifyDrone();
                    break;
                case 2: modifyCustomer();
                    break;
                case 3: modifyStation();
                    break; 
                default:
                    Console.WriteLine("Did not receive proper command!\n");
                    break;
            }
        }




        void modifyDrone()
        {
            Console.WriteLine("Please enter drone Id and new model\n");
            int _id = 0;
            Int32.TryParse(Console.ReadLine(), out _id);
            string _model = Console.ReadLine();
            try
            {
                busiAccess.modifyDrone(_id, _model);
            }
            catch(IBL.BO.EXNotFoundPrintException exception)
            {
                exception.printException();
            }

        }
        void modifyCustomer()
        {
            Console.WriteLine("Please enter Customer Id\n");
            int _id = 0;
            Int32.TryParse(Console.ReadLine(), out _id);
            Console.WriteLine("Pls enter Customers new Name, and new Phone number \n" +
                "(or enter \"0\" if you do not want to change a field\n");
            string _name = Console.ReadLine();
            string _phone = Console.ReadLine();
            if (_name == "0") _name = "";
            if (_phone == "0") _phone = "";
            try
            {
                busiAccess.modifyCust(_id, _name, _phone);
            }
            catch(IBL.BO.EXNotFoundPrintException exception)
            {
                exception.printException();
            }
        }
        void modifyStation()
        {
            Console.WriteLine("Please enter Station Id\n");
            int _id = 0;
            Int32.TryParse(Console.ReadLine(), out _id);
            Console.WriteLine("Pls enter Station's new Name, and new number of charging slots \n" +
                "(or enter \"0\" if you do not want to change a field\n");
            int _name = 0;
            Int32.TryParse(Console.ReadLine(), out _name);
            int _numChargingSlots = 0;
            Int32.TryParse(Console.ReadLine(), out _numChargingSlots);
            try
            {
                busiAccess.modifyStation(_id, _name, _numChargingSlots);
            }
            catch(IBL.BO.EXNotFoundPrintException exception)
            {
                exception.printException();
            }
        }

        void updateActions()
        {

            Console.WriteLine("What would you like to update ?\n" +
                "1: Assign a parcel to a drone\n" +
                "2: Collect a parcel with its assigned drone\n" +
                "3: Deliver a parcel to a customer\n" +
                "4: Send a drone to a charging station\n" +
                "5: Free a drone from a charging station\n");

            int choice = 0;
            int id = 0;
            Int32.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the ID of the drone you would like to assign:\n");
                    Int32.TryParse(Console.ReadLine(), out id);
                    try
                    {
                        busiAccess.assignParcel(id);
                    }
                    catch(IBL.BO.EXNotFoundPrintException exception)
                    {
                        if (exception.ItemName != "not available")
                            exception.printException();
                        else
                            exception.printNotAvailableException();
                    }
                    catch(IBL.BO.EXPrintException exception) {exception.Print();}
                    break;
                case 2:
                    Console.WriteLine("Enter the ID of the drone you want to collect:\n");
                    Int32.TryParse(Console.ReadLine(), out id);
                    try
                    {
                        busiAccess.collectParcel(id);
                    }
                    catch (IBL.BO.EXNotFoundPrintException exception) {exception.printException();}
                    catch (IBL.BO.EXPrintException exception){exception.Print();}
                    break;
                case 3:
                    Console.WriteLine("Enter the ID of the drone you want to deliver:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    try
                    {
                        busiAccess.deliverParcel(id);
                    }
                    catch (IBL.BO.EXNotFoundPrintException exception) { exception.printException();}
                    catch (IBL.BO.EXPrintException exception) { exception.Print();}
                    break;
                case 4:
                    Console.WriteLine("Enter the ID of the drone you want to charge:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    try
                    {
                        busiAccess.chargeDrone(id);
                    }
                    catch (IBL.BO.EXNotFoundPrintException exception)
                    {
                        if (exception.ItemName != "not available")
                            exception.printException();
                        else
                            exception.printNotAvailableException();
                    }
                    catch (IBL.BO.EXPrintException exception) { exception.Print();}

                    break;
                case 5:
                    Console.WriteLine("Enter the ID of the drone you want to free:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    double minutesCharged = 0;
                    Console.WriteLine("Enter the num of minutess the drone charged:\n ");
                    Double.TryParse(Console.ReadLine(), out minutesCharged);
                    try
                    {
                        busiAccess.freeDrone(id, minutesCharged);
                    }
                    catch (IBL.BO.EXNotFoundPrintException exception) { exception.printException(); }
                    catch (IBL.BO.EXPrintException exception) { exception.Print(); }
                    break;
                default:
                    break;
            }
        }







        //end of class
    }
}

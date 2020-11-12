using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robot3DWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // variables declaration 
        int xLimitInferior = 0, yLimitInferior = 0, zLimitInferior = 0,
            xLimitSuperior = 0, yLimitSuperior = 0, zLimitSuperior = 0,
            xLastSet = 0, yLastSet = 0, zLastSet = 0,
            xActual = 0, yActual = 0, zActual = 0;
        bool wasSet;

        List<string> knownCommandsList = new List<string>() { "SET", "MOVE", "3DMOVE", "PRINT", "RESET", "QUIT" };

        public MainWindow()
        {
            InitializeComponent();
            SetStartingVariables();
        }

        private void commandTextBox_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                System.Windows.Controls.TextBox texboxControl = sender as System.Windows.Controls.TextBox;//convert sender to Textbox object, in order to have access to it's text property
                string text = texboxControl.Text;

                string[] linesFromCommandWindow = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string lastCommand = linesFromCommandWindow[linesFromCommandWindow.Length - 1];

                commandTextbox.Text = commandTextbox.Text + Environment.NewLine;//add new line
                commandTextbox.SelectionStart = commandTextbox.Text.Length;  //move coursor at the end of text
                commandTextbox.SelectionLength = 0;
             

                string[] commandInformation = lastCommand.Split(' ');

                try
                {
                    if (knownCommandsList.Contains(commandInformation[0]))
                    {
                        if (commandInformation[0] == "SET")
                        {


                            string xInput = commandInformation[1].Substring(0, commandInformation[1].Length - 1); //extract value as string , except last character(,)
                            int xValueFromCommand = Convert.ToInt32(xInput);

                            string yInput = commandInformation[2].Substring(0, commandInformation[2].Length - 1);
                            int yValueFromCommand = Convert.ToInt32(yInput);

                            string zInput = commandInformation[3];
                            int zValueFromCommand = Convert.ToInt32(zInput);


                            //LIMIT CHECK MECHANISM
                            if ((xLimitInferior <= xValueFromCommand && xValueFromCommand <= xLimitSuperior) &&
                                (yLimitInferior <= yValueFromCommand && yValueFromCommand <= yLimitSuperior) &&
                                (zLimitInferior <= zValueFromCommand && zValueFromCommand <= zLimitSuperior))
                            {
                                xLastSet = xValueFromCommand;
                                xActual = xValueFromCommand;

                                yLastSet = yValueFromCommand;
                                yActual = yValueFromCommand;

                                zLastSet = zValueFromCommand;
                                zActual = zValueFromCommand;

                                wasSet = true;

                                outputTextbox.Text = "";
                            }

                            else
                            {
                                outputTextbox.Text  = "At least one of the values exceeds the limits! Please set valid values.";
                            }

                        }


                        if (commandInformation[0] == "PRINT")
                        {
                            if (wasSet == true)
                            {
                                outputTextbox.Text  = "Current location of robot: X : " + xActual + " Y : " + yActual + " Z : " + zActual;
                            }
                            else
                            {
                                outputTextbox.Text = "???, ???, ???";
                            }
                        }

                        if (commandInformation[0] == "QUIT")
                        {

                            Environment.Exit(0);
                        }

                        if (commandInformation[0] == "RESET")
                        {
                            if (wasSet == true)
                            {
                                xActual = xLastSet;

                                yActual = yLastSet;

                                zActual = zLastSet;

                                outputTextbox.Text = "";
                            }
                            else
                            {
                                outputTextbox.Text = "Please perform a SET command before a RESET command.";
                            }

                        }

                        if (commandInformation[0] == "3DMOVE")
                            if (wasSet == true)
                            {
                                string xNewMoves = commandInformation[1].Substring(0, commandInformation[1].Length - 1);
                                int xMoves = Convert.ToInt32(xNewMoves);

                                string yNewMoves = commandInformation[2].Substring(0, commandInformation[2].Length - 1);
                                int yMoves = Convert.ToInt32(yNewMoves);

                                string zNewMoves = commandInformation[3].Substring(0, commandInformation[3].Length);
                                int zMoves = Convert.ToInt32(zNewMoves);

                                if ((xLimitInferior <= xMoves + xActual && xMoves + xActual <= xLimitSuperior) &&
                                    (yLimitInferior <= yMoves + yActual && yMoves + yActual <= yLimitSuperior) &&
                                    (zLimitInferior <= zMoves + zActual && zMoves + zActual <= zLimitSuperior))
                                {
                                    xActual = xLastSet + xMoves;
                                    yActual = yLastSet + yMoves;
                                    zActual = zLastSet + zMoves;

                                    outputTextbox.Text = "";
                                }

                                else
                                {
                                    outputTextbox.Text = "Limits exceed the boundaries. Please insert a valid command";
                                }

                            }
                            else
                            {
                                outputTextbox.Text = "Please perform a SET command before a 3DMOVE command.";
                            }

                        if (commandInformation[0] == "MOVE")
                        {
                            if (wasSet == true)
                            {
                                string NewSteps = commandInformation[1].Substring(0, commandInformation[1].Length - 1);
                                int Moves = Convert.ToInt32(NewSteps);

                                //LIMIT CHECK MECHANISM
                                if ((xLimitInferior <= xActual + Moves && xActual + Moves <= xLimitSuperior) &&
                                (yLimitInferior <= yActual + Moves && yActual + Moves <= yLimitSuperior) &&
                                (zLimitInferior <= zActual + Moves && zActual + Moves <= zLimitSuperior))
                                {
                                    if (commandInformation[2] == "X")
                                    {
                                        xActual = xActual + Moves;
                                    }

                                    if (commandInformation[2] == "Y")
                                    {
                                        yActual = yActual + Moves;
                                    }

                                    if (commandInformation[2] == "Z")
                                    {
                                        zActual = zActual + Moves;
                                    }

                                    outputTextbox.Text = "";
                                }
                                else
                                {
                                    outputTextbox.Text = "At least one of the values exceeds the limits! Please set valid values.";
                                }
                            }
                            else
                            {
                                outputTextbox.Text = "Please perform a SET command before a MOVE command.";
                            }
                        }
                    }
                    else
                    {
                        outputTextbox.Text = "Please enter a command that starts with SET, MOVE, 3DMOVE, PRINT, RESET, QUIT";
                    }
                }
                catch (Exception ex)
                {
                    outputTextbox.Text = "Please enter a valid command.";
                }

                



            }
        }

        private void SetStartingVariables()
        {
            // extract and set limits from file
            string[] lines = System.IO.File.ReadAllLines(@"resources/limits.txt");//System.IO.File.ReadAllLines(@"D:\limits.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                string limitInformation = lines[i];
                string[] limit = limitInformation.Split(' ');

                if (limit[0] == "X")
                {
                    xLimitInferior = Int32.Parse(limit[1]);
                    xLimitSuperior = Int32.Parse(limit[2]);

                }

                if (limit[0] == "Y")
                {
                    yLimitInferior = Int32.Parse(limit[1]);
                    yLimitSuperior = Int32.Parse(limit[2]);
                }

                if (limit[0] == "Z")
                {
                    zLimitInferior = Int32.Parse(limit[1]);
                    zLimitSuperior = Int32.Parse(limit[2]);
                }

            }

            //handle commands

            wasSet = false; //FLAG THAT INDICATES IF A set OPPERATION WAS DONE SUCCESFULLY 
        }

      
    }
}

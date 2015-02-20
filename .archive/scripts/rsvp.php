<?php

$errorString = '';

if($_POST['submit'] == "Submit RSVP")
{
    $email = $_POST['email'];
    $password = $_POST['password'];
    $comments = $_POST['comments'];
    $attending = $_POST['attending'];


    // Sanitize the given email address
    $email = filter_var($email, FILTER_SANITIZE_EMAIL);
    
    // Validate the given email address
    $email = filter_var($email, FILTER_VALIDATE_EMAIL);
    
    if (empty($email))
    {
        $errorMessage .= "<li>Please enter a valid email address.</li>";
    }
    
    
    // Verify the password has been entered
    if (empty($password))
    {
        $errorMessage .= "<li>Please enter the password.</li>";
    }
    else
    {           
        // Verify the password is correct             
        $hashed = crypt($password, '$2y$04$thisIsARidiculousSalt.');
        
        if ($hashed != '$2y$04$thisIsARidiculousSalt.bfS.HDx7WuNMNAsHq7etnW2zxLAnA2C')
        {
            $errorMessage .= '<li>The password is incorrect.</li>';
        }
    }

    $keyName = 'first';
    $keys = array();

    // Collect all of the "firstN" keys
    foreach($_POST as $key => &$val) 
    {
        if (strpos($key, $keyName) === 0) 
        {
            $keys[] = $key;
        }
    }

    // Verify that at least one attendee was given
    if (count($keys) == 0)
    {
        $errorMessage .= "<li>Please add at least one attendee to your party.</li>";
    }
    

    // Validate each attendee
    foreach ($keys as $key) 
    {
        $index = substr($key, strlen($keyName));

        $first = $_POST['first' . $index];
        $last = $_POST['last' . $index];
        $age = $_POST['age' . $index];

        if (empty($first))
        {
            $errorMessage .= '<li>Attendee ' . $index . ' has no first name.</li>';
        }
        if (empty($last))
        {
            $errorMessage .= '<li>Attendee ' . $index . ' has no last name.</li>';
        }
    }


    // If we've passed all checks, then let's go ahead and generate the RSVP
    if (empty($errorMessage))
    {
        $body = '';
        
        // Iterate over the "firstN" keys and generate attendee list
        foreach ($keys as $key) 
        {
            $index = substr($key, -1);

            $first = $_POST['first' . $index];
            $last = $_POST['last' . $index];
            $food = $_POST['food' . $index];
            $age = $_POST['age' . $index];
            
            $body .= $last . "\t" . $first . "\t" . $age . "\t" . $food . "\n";
        }
                
        $body .= $email . "\n";
        $body .= $comments . "\n";
        $body .= $attending . "\n";
        
        $subject = "RSVP: ";
        $subject .= $attending;
        
        mail("jamietimski@gmail.com", $subject, $body, 'From: rsvp@timski.info');
        
        header("Location: submitted.html");
    }
}

?>
<html>
    <head>
        <title>Timski</title>
        <link rel="stylesheet" type="text/css" href="style.css" />
        <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
        <script type="text/javascript" src="jquery-1.8.3.js"></script>
    </head>
    <body>
        <table width="90%">
            <tbody>
                <tr class="cell">
                    <td>
                        <a name="Top"></a>
                        <table class="cell">
                            <tr class="cell">
                                <td>
                                    <h1>RSVP</h1>
                                </td>
                            </tr>
                        </table>
                        
                        
                        <?php
                        if (!empty($errorMessage))
                        {
                            echo '<p style="color:#dd0033"><strong>One or more errors encountered while attempting to submit the form:</strong></p>';
                            echo '<ul style="color:#dd0033">' . $errorMessage . '</ul>';
                        }
                        ?>
                        
                        
                        <h3>Instructions</h3>
                        <ol>
                            <li>For each attendee in your party (<strong>including yourself</strong>), click the "Add Attendee" button and fill out the appropriate fields
                            <ul>
                                <li>
                                    <strong>First Name:</strong> enter the first name of the attendee
                                </li>
                                <li>
                                    <strong>Last Name:</strong> enter the last name of the attendee
                                </li>
                                <li>
                                    <strong>Child/Adult:</strong> select "Child" if the attendee is age 12 or under; otherwise, select "Adult"
                                </li>
                                <li>
                                    To remove an attendee entirely, click the "Remove Attendee" button alow that attendee
                                </li>
                            </ul>
                        </li>
                            <li>
                                <strong>Attending?</strong>
                                <ul>
                                    <li>select "Yes" if you will be attending the most awesome wedding of 2013!</li>
                                    <li>select "No" if you will not be attending</li>
                                </ul>
                            </li>
                            <li>
                                <strong>Primary contact email address:</strong> enter an email address that may be used to contact all of the attendees
                            </li>
                            <li>
                                <strong>Password:</strong> enter the password, found in the Christmas card you should have received from Timski and Jamie
                                <ul>
                                    <li>if you have lost the password, contact Timski or Jamie to get it</li>
                                </ul>
                            </li>
                            <li>
                                <strong>Comments/Requests:</strong> enter any comments/requests/questions you may have
                            </li>
                            <li>
                                Click "Submit RSVP" to submit the form. If any errors occur, this page will reload with the errors displayed in red. If
                                no errors occur, you should be redirected to another page.
                            </li>
                        </ol>

                        <table id="person" style="display: none">
                            <tr>
                                <td style="width: 20%">First Name:</td>
                                <td>
                                    <input id="first" name="first" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td>Last Name:</td>
                                <td>
                                    <input id="last" name="last" type="text" />
                                </td>
                            </tr>
                            <tr>
                                <td>Food allergies/restrictions:</td>
                                <td>
                                    <input id="food" name="food" type="text" size="50" />
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2">Child/Adult:</td>
                                <td>
                                    <input id="adult" name="age" type="radio" value="adult" checked="checked" />
                                    <label id="adultLabel" for="adult">Adult (over 12)</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="child" name="age" type="radio" value="child" />
                                    <label id="childLabel" for="child">Child (12 or under)</label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="0">
                                    <input id="remove" type="button" value="Remove Attendee"  />
                                </td>
                            </tr>
                        </table>
                        

                        

                        <h3>Form</h3>
                        
                        <form id="form" action="rsvp.php" method="post">

                            <?php
                            if (!empty($errorMessage))
                            {
                                if (count($keys) > 0)
                                {
                                    foreach ($keys as $key)
                                    {
                                        $index = substr($key, strlen($keyName));
                                        
                                        echo '<table id="person' . $index . '">';
                                        echo '    <tr>';
                                        echo '        <td style="width: 20%">First Name:</td>';
                                        echo '        <td>';
                                        echo '            <input id="first' . $index . '" name="first' . $index . '" type="text" value="' . $_POST['first' . $index] . '" />';
                                        echo '        </td>';
                                        echo '    </tr>';
                                        echo '    <tr>';
                                        echo '        <td>Last Name:</td>';
                                        echo '        <td>';
                                        echo '            <input id="last' . $index . '" name="last' . $index . '" type="text" value="' . $_POST['last' . $index] . '" />';
                                        echo '        </td>';
                                        echo '    </tr>';
                                        echo '    <tr>';
                                        echo '        <td>Food allergies/restrictions:</td>';
                                        echo '        <td>';
                                        echo '            <input id="food' . $index . '" name="food' . $index . '" type="text" value="' . $_POST['food' . $index] . '" />';
                                        echo '        </td>';
                                        echo '    </tr>';
                                        echo '    <tr>';
                                        echo '        <td rowspan="2">Child/Adult:</td>';
                                        echo '        <td>';
                                        echo '            <input id="adult' . $index . '" name="age' . $index . '" type="radio" value="adult" />';
                                        echo '            <label id="adultLabel' . $index . '" for="adult' . $index . '">Adult (over 12)</label>';
                                        echo '        </td>';
                                        echo '    </tr>';
                                        echo '    <tr>';
                                        echo '        <td>';
                                        echo '            <input id="child' . $index . '" name="age' . $index . '" type="radio" value="child" />';
                                        echo '            <label id="childLabel' . $index . '" for="child' . $index . '">Child (12 or under)</label>';
                                        echo '        </td>';
                                        echo '    </tr>';                                                  
                                        echo '    <tr>';
                                        echo '        <td colspan="0">';
                                        echo '            <input id="remove' . $index . '" type="button" value="Remove Attendee" onclick="removePerson(\'#person' . $index . '\')" />';
                                        echo '        </td>';
                                        echo '    </tr>';
                                        echo '</table>';
                                        
                                    }
                                }
                            }
                            ?>
                        
                            <input id="add" type="button" value="Add Attendee" />

                            <table id="other">                                
                                <tr>
                                    <td rowspan="2">Attending?</td>
                                    <td>
                                        <input id="yes" name="attending" type="radio" value="Yes" checked="checked" />
                                        <label id="yesLabel" for="yes">Yes</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input id="no" name="attending" type="radio" value="No" />
                                        <label id="noLabel" for="no">No</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%">Primary contact email address:</td>
                                    <td>
                                        <input id="email" name="email" type="text" value="<?=$email;?>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password:</td>
                                    <td>
                                        <input id="password" name="password" type="password" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Comments/Requests:</td>
                                    <td>
                                        <input id="comments" name="comments" type="text" size="50" value="<?=$comments;?>"  />
                                    </td>
                                </tr>
                            </table>
                            <input id="submit" name="submit" type="submit" value="Submit RSVP" />
                            <br />

                        </form>



                        <script type="text/javascript" src="rsvp.js"></script>

                        <span id="hi" style="display: none">Hi!</span>
                        
                    </td>
                </tr>
            </tbody>
        </table>
    </body>
</html>
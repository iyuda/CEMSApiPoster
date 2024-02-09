<?php


$results = array();
$results["error"] = "";
$results["id"] = "";

$servername = "localhost";

$username = "root";

$password = "secret";

$dbname = "ios";

$payload = file_get_contents('php://input');
$id = $_GET['id'];

$sqlInsert = "INSERT INTO json_pcr (id, agency, data)
 VALUES ('" . $id . "', '" . $agency . "', '" . $payload . "')";
$sqlInsert=$sqlInsert." ON DUPLICATE KEY UPDATE data='" . $payload . "'";$sqlID = "SELECT @last_id as id";

$conn = new mysqli($servername, $username, $password, $dbname);
$err = $conn->connect_error;

if ($err) 
  $results["error"] = $err;

else
{


  $return = $conn->query($sqlInsert);
  $err = mysqli_error($conn);     //$conn->error

  if ($err)
    $results["error"] = $err;
  else
  {
    $return = $conn->query($sqlID);
    $err = mysqli_error($conn);     //$conn->error

    if ($err)
      $results["error"] = $err;
    else
      while ($row = $return->fetch_assoc()) 
        $results["id"] = $row["id"];
  }

}




$conn->close();

echo json_encode($results);
file_get_contents('http://localhost:53184/api/CEMS/GetPcrFromIOS?pcr_id='.$id );
?>
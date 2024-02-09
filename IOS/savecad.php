<?php


$results = array();
$results["error"] = "";
$results["id"] = "";

$servername = "localhost";

$username = "root";

$password = "secret";

$dbname = "ios";

$payload = file_get_contents('php://input');

$sqlInsert = "INSERT INTO json_cad (id, data)
 VALUES ('', '" . $payload . "')";

$sqlID = "SELECT @last_id as id";

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

?>
<?php

$servername = "localhost";

$username = "root";

$password = "secret";

$dbname = "ios";

$id = $_GET['id'];

file_get_contents('http://localhost:53184/api/CEMS/GetCadFromEMS?cad_id='.$id );
$sqlSelect = "select CAST(data AS CHAR) as data from json_cad where id = '" . $id . "' and year(received)<>1970 and year(sent)=1970";

$conn = new mysqli($servername, $username, $password, $dbname);

$return = $conn->query($sqlSelect);
$err = mysqli_error($conn);     //$conn->error
if ($return->num_rows > 0) {
	while ($row = $return->fetch_assoc()) {
		echo $row["data"];
	}
	$sqlUpdate = "update json_cad set sent=".date('Y-m-d h:i:s')." where id = '" . $id . "'";
	$return = $conn->query($sqlUpdate);
}
 
$conn->close();

?>

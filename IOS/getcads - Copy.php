<?php
$cads = array();

$bus = $_GET['bus'];

$cads["bus"] = $bus;

$cad = array();
$cad["created"] = "12/21/2016 10:32";
$cad["id"] = "df46f7f1-a647-4214-84d0-986b833e2141";

array_push($cads, $cad);

$cad = array();
$cad["created"] = "12/21/2016 14:16";
$cad["id"] = "78e4ff93-1d8a-4b79-bcea-511bbd3af164";

array_push($cads, $cad);

$cad = array();
$cad["created"] = "12/22/2016 9:42";
$cad["id"] = "4b2edec9-0f71-460f-822c-ecee445949c4";

array_push($cads, $cad);


// all in one line:

array_push($cads, array( "created" => "12/23/2016 18:10", "id" => "1cac550b-14bf-4a83-9b07-6dfdb497f578" ));

echo json_encode($cads);
?>


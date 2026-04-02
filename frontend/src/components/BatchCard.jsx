import React from "react";
import { Card, Button, Badge } from "react-bootstrap";

const BatchCard = ({ batch }) => {
  return (
    <Card className="shadow h-100">
      <Card.Header className="fw-bold">{batch.batchName || "No Name"}</Card.Header>
      <Card.Body>
        <p><Badge bg="primary">Batch ID: {batch.batchId}</Badge></p>
        <p>Students: {batch.studentCount || 0}</p>
        <p>Avg Perf: {batch.avgScore || 0}%</p>
      </Card.Body>
      <Card.Footer className="text-center">
        <Button variant="warning">View More</Button>
      </Card.Footer>
    </Card>
  );
};

export default BatchCard;
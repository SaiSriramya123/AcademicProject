import React from "react";
import { Card } from "react-bootstrap";

const SummaryCard = ({ title, count, color }) => {
  return (
    <Card className="shadow" style={{ backgroundColor: color, color: "#fff" }}>
      <Card.Body>
        <h6 className="fw-bold">{title}</h6>
        <h2>{count}</h2>
      </Card.Body>
    </Card>
  );
};

// ✅ Default export
export default SummaryCard;
import React, { useEffect, useState } from "react";

import axios from "axios";

import { useParams } from "react-router-dom";

const StudentDetails = () => {

  const { batchId } = useParams();

  const [students, setStudents] = useState([]);

  useEffect(() => {

    axios

      .get(`https://localhost:7157/api/performance/batch/${batchId}`)

      .then((res) => {

        const data = Array.isArray(res.data) ? res.data : [];

        setStudents(data);

      })

      .catch(() => setStudents([]));

  }, [batchId]);

  return (
<div className="container mt-4">
<h3>Student Details</h3>
<table className="table mt-3">
<thead>
<tr>
<th>Name</th>
<th>Enrollment ID</th>
<th>Avg Score</th>
</tr>
</thead>
<tbody>

          {students.map((s, i) => (
<tr key={i}>
<td>{s.name}</td>
<td>{s.enrollmentId}</td>
<td>{s.avgScore}%</td>
</tr>

          ))}
</tbody>
</table>
</div>

  );

};

export default StudentDetails;
 
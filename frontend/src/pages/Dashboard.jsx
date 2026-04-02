import React, { useEffect, useState } from "react";

import axios from "axios";

import { useNavigate } from "react-router-dom";

const Dashboard = () => {

  const instructorId = 1001;

  const navigate = useNavigate();

  const [batches, setBatches] = useState([]);

  const [showBatches, setShowBatches] = useState(false);

  const [totalStudents, setTotalStudents] = useState(0);

  const [avgScore, setAvgScore] = useState(0);

  // 🔥 API CALL

  useEffect(() => {

    axios

      .get(`https://localhost:7157/api/performance/instructor-batches/${instructorId}`)

      .then((res) => {

        const data = Array.isArray(res.data) ? res.data : [];

        setBatches(data);

        const total = data.reduce(

          (sum, b) => sum + (b.studentCount || 0),

          0

        );

        setTotalStudents(total);

        const avg =

          data.length > 0

            ? data.reduce((sum, b) => sum + (b.avgScore || 0), 0) /

              data.length

            : 0;

        setAvgScore(avg.toFixed(1));

      })

      .catch(() => setBatches([]));

  }, []);

  return (
<div className="container mt-4">

      {/* 🔷 HEADER */}
<div className="d-flex justify-content-between align-items-center mb-4">
<h3>EDU TRACK</h3>
<div className="text-end">
<p className="mb-0 fw-bold">Instructor Dashboard</p>
<p className="mb-0">📞 +91 98765 43210</p>
<span className="badge bg-primary">ID: {instructorId}</span>
</div>
</div>

      {/* 🔷 TITLE */}
<h4>Instructor Dashboard</h4>
<p className="text-muted">

        Overview of all your assigned batches and performance
</p>

      {/* 🔷 TOP CARDS */}
<div className="row mb-4">

        {/* 🔵 TOTAL BATCHES */}
<div className="col-md-4 position-relative">
<div

            className="card text-white bg-primary p-3 shadow"

            style={{ cursor: "pointer" }}

            onClick={() => setShowBatches(!showBatches)}
>
<h5>Total Batches</h5>
<h2>{batches.length}</h2>
</div>

          {/* 🔥 DROPDOWN BATCH LIST */}

          {showBatches && (
<div className="card p-2 mt-2 shadow">
<strong>Assigned Batches:</strong>
<ul className="mb-0">

                {batches.length > 0 ? (

                  batches.map((b) => (
<li key={b.batchId}>

                      {b.batchName} ({b.studentCount})
</li>

                  ))

                ) : (
<li>No batches</li>

                )}
</ul>
</div>

          )}
</div>

        {/* 🟢 TOTAL STUDENTS */}
<div className="col-md-4">
<div className="card text-white bg-success p-3 shadow">
<h5>Total Students</h5>
<h2>{totalStudents}</h2>
</div>
</div>

        {/* 🟡 AVG SCORE */}
<div className="col-md-4">
<div className="card text-white bg-warning p-3 shadow">
<h5>Average Score</h5>
<h2>{avgScore}%</h2>
</div>
</div>
</div>

      {/* 🔷 COURSE PERFORMANCE */}
<h5>Course Performance</h5>
<div className="mt-3">

        {batches.length > 0 ? (

          batches.map((b) => (
<div key={b.batchId} className="card mb-3 shadow">
<div className="card-header d-flex justify-content-between">
<div>
<h6 className="mb-0">{b.batchName}</h6>
<small className="text-muted">Course Module</small>
</div>
<span className="badge bg-primary">

                  {b.batchCode || "B001"}
</span>
</div>
<div className="card-body d-flex justify-content-around text-center">
<div>
<h5>{b.studentCount || 0}</h5>
<small>Students</small>
</div>
<div>
<h5>{b.avgScore || 0}%</h5>
<small>Avg Performance</small>
</div>
</div>
<div className="text-center pb-3">
<button

                  className="btn btn-outline-primary"

                  onClick={() => navigate(`/students/${b.batchId}`)}
>

                  View More →
</button>
</div>
</div>

          ))

        ) : (
<p>No performance data available</p>

        )}
</div>
</div>

  );

};

export default Dashboard;
 
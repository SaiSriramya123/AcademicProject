import React, { useEffect, useState } from 'react';

import 'bootstrap/dist/css/bootstrap.min.css';

import { getInstructorBatches } from "../services/Api"; // Fixed path: one level back

const InstructorDashboard = () => {

    const [batches, setBatches] = useState([]);

    const [loading, setLoading] = useState(true);

    // Profile details from your Figma design

    const instructor = {

        id: "INS-2024-001",

        email: "sarah.johnson@edutrack.com",

        phone: "+1 (555) 123-4567"

    };

    useEffect(() => {

        const loadData = async () => {

            try {

                const data = await getInstructorBatches(instructor.id);

                setBatches(data);

                setLoading(false);

            } catch (error) {

                console.error("Error loading dashboard data");

                setLoading(false);

            }

        };

        loadData();

    }, []);

    const totalStudents = batches.reduce((sum, b) => sum + (b.students || 0), 0);

    if (loading) return <div className="p-5 text-center fw-bold">Loading Edu Track...</div>;

    return (
<div className="min-vh-100" style={{ backgroundColor: '#fcfcfc', padding: '40px' }}>

            {/* Header Section */}
<div className="d-flex justify-content-between align-items-start mb-5">
<div>
<h1 className="fw-bold mb-1" style={{ color: '#1a1a1a' }}>Edu Track</h1>
<p className="text-muted">View the performance of the batches assigned to you</p>
</div>
<div className="d-flex align-items-center">
<div className="text-muted" style={{ fontSize: '13px', textAlign: 'right' }}>
<div className="fw-bold text-dark">ID: {instructor.id}</div>
<div>{instructor.email}</div>
<div>{instructor.phone}</div>
</div>
<div className="ms-3 bg-primary bg-opacity-10 rounded-circle d-flex align-items-center justify-content-center" style={{ width: '45px', height: '45px' }}>
<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="#0d6efd" viewBox="0 0 16 16">
<path d="M8 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6m2-3a2 2 0 1 1-4 0 2 2 0 0 1 4 0m4 8c0 1-1 1-1 1H3s-1 0-1-1 1-4 6-4 6 3 6 4m-1-.004c-.001-.246-.154-.986-.832-1.664C11.516 10.68 10.289 10 8 10s-3.516.68-4.168 1.332c-.678.678-.83 1.418-.832 1.664z"/>
</svg>
</div>
</div>
</div>

            {/* Summary Top Cards */}
<div className="row g-4 mb-5">
<div className="col-md-6">
<div className="p-4 rounded-3 shadow-sm h-100 border-0" style={{ backgroundColor: '#f5f8ff', borderLeft: '6px solid #4a90e2' }}>
<div className="d-flex justify-content-between align-items-start">
<div>
<p className="fw-bold mb-4" style={{ color: '#4a90e2', fontSize: '14px' }}>Batches Assigned</p>
<h1 className="fw-bold mb-2" style={{ fontSize: '3rem' }}>{batches.length}</h1>
<p className="text-muted small mb-0">Active batches under your supervision</p>
</div>
<span style={{ opacity: 0.3 }}>
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#4a90e2" strokeWidth="2"><path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"></path><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"></path></svg>
</span>
</div>
</div>
</div>
<div className="col-md-6">
<div className="p-4 rounded-3 shadow-sm h-100 border-0" style={{ backgroundColor: '#f6fbf7', borderLeft: '6px solid #52c41a' }}>
<div className="d-flex justify-content-between align-items-start">
<div>
<p className="fw-bold mb-4" style={{ color: '#52c41a', fontSize: '14px' }}>Total Students</p>
<h1 className="fw-bold mb-2" style={{ fontSize: '3rem' }}>{totalStudents}</h1>
<p className="text-muted small mb-0">Students across all batches</p>
</div>
<span style={{ opacity: 0.3 }}>
<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#52c41a" strokeWidth="2"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
</span>
</div>
</div>
</div>
</div>

            {/* Grid Title */}
<h5 className="fw-bold mb-4">Your Batches</h5>

            {/* Batch List */}
<div className="row g-4">

                {batches.map((batch) => (
<div key={batch.batchId} className="col-md-4">
<div className="card border-0 shadow-sm p-4 rounded-4 h-100 position-relative">
<span className="position-absolute top-0 end-0 m-3 px-2 py-1 rounded bg-light border text-muted fw-bold" style={{ fontSize: '10px' }}>

                                {batch.batchId}
</span>
<h5 className="fw-bold mt-2" style={{ color: '#333' }}>{batch.batchName}</h5>
<p className="text-muted small mb-4">{batch.department}</p>
<div className="d-flex justify-content-between align-items-center mb-2">
<span className="text-muted small">Students</span>
<span className="fw-bold text-dark">{batch.students}</span>
</div>
<div className="d-flex justify-content-between align-items-center mb-4">
<span className="text-muted small">Avg. Performance</span>
<span className="fw-bold" style={{ color: '#27ae60' }}>{batch.avgPerformance}%</span>
</div>
<div className="text-muted border-top pt-3 mt-auto" style={{ fontSize: '11px' }}>

                                {batch.schedule}
</div>
</div>
</div>

                ))}
</div>
</div>

    );

};

export default InstructorDashboard;
 
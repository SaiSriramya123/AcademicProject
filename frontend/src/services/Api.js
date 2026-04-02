import axios from 'axios';

// The port 7157 is taken from your Swagger URL screenshot

const API_BASE_URL = 'https://localhost:7157/api/Performance';

export const getInstructorBatches = async () => {

    try {

        // This calls the endpoint shown in your Swagger UI

        const response = await axios.get(`${API_BASE_URL}/instructor-batches/INS-2024-001`);

        return response.data;

    } catch (error) {

        console.error("API Error:", error);

        throw error;

    }

};
 
// Mock API Service for Programs and Courses

// Mock Programs Data
export const mockPrograms = [
  {
    id: 1,
    name: 'Bachelor of Computer Science',
    qualification: 'B.Sc',
    duration: '4 Years',
    description: 'Comprehensive program covering software development, algorithms, and data structures.',
    category: 'Technology',
  },
  {
    id: 2,
    name: 'Master of Business Administration',
    qualification: 'MBA',
    duration: '2 Years',
    description: 'Advanced business management and leadership training for aspiring executives.',
    category: 'Business',
  },
  {
    id: 3,
    name: 'Bachelor of Engineering',
    qualification: 'B.E',
    duration: '4 Years',
    description: 'Engineering fundamentals with specializations in various disciplines.',
    category: 'Engineering',
  },
  {
    id: 4,
    name: 'Master of Data Science',
    qualification: 'M.Sc',
    duration: '2 Years',
    description: 'Advanced analytics, machine learning, and big data technologies.',
    category: 'Technology',
  },
  {
    id: 5,
    name: 'Bachelor of Arts',
    qualification: 'B.A',
    duration: '3 Years',
    description: 'Liberal arts education with focus on humanities and social sciences.',
    category: 'Arts',
  },
  {
    id: 6,
    name: 'Master of Public Health',
    qualification: 'MPH',
    duration: '2 Years',
    description: 'Public health management, epidemiology, and health policy.',
    category: 'Healthcare',
  },
];

// Mock Courses Data
export const mockCourses = [
  {
    id: 1,
    name: 'Full Stack Web Development',
    instructor: 'Dr. Sarah Johnson',
    rating: 4.8,
    students: 12543,
    duration: '12 Weeks',
    category: 'Web Development',
    thumbnail: 'https://images.unsplash.com/photo-1498050108023-c5249f4df085',
    level: 'Intermediate',
  },
  {
    id: 2,
    name: 'Machine Learning Fundamentals',
    instructor: 'Prof. Michael Chen',
    rating: 4.9,
    students: 8932,
    duration: '10 Weeks',
    category: 'Data Science',
    thumbnail: 'https://images.unsplash.com/photo-1555949963-ff9fe0c870eb',
    level: 'Advanced',
  },
  {
    id: 3,
    name: 'Digital Marketing Mastery',
    instructor: 'Jessica Williams',
    rating: 4.7,
    students: 15621,
    duration: '8 Weeks',
    category: 'Marketing',
    thumbnail: 'https://images.unsplash.com/photo-1460925895917-afdab827c52f',
    level: 'Beginner',
  },
  {
    id: 4,
    name: 'UI/UX Design Principles',
    instructor: 'David Martinez',
    rating: 4.9,
    students: 11234,
    duration: '6 Weeks',
    category: 'Design',
    thumbnail: 'https://images.unsplash.com/photo-1561070791-2526d30994b5',
    level: 'Intermediate',
  },
  {
    id: 5,
    name: 'Python Programming',
    instructor: 'Dr. Emily Taylor',
    rating: 4.8,
    students: 19876,
    duration: '14 Weeks',
    category: 'Programming',
    thumbnail: 'https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5',
    level: 'Beginner',
  },
  {
    id: 6,
    name: 'Cloud Computing with AWS',
    instructor: 'Robert Anderson',
    rating: 4.7,
    students: 9543,
    duration: '10 Weeks',
    category: 'Cloud',
    thumbnail: 'https://images.unsplash.com/photo-1451187580459-43490279c0fa',
    level: 'Advanced',
  },
];

// API Functions
export const api = {
  // Get all programs
  getPrograms: async () => {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(mockPrograms);
      }, 500);
    });
  },

  // Get all courses
  getCourses: async () => {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(mockCourses);
      }, 500);
    });
  },

  // Mock login - Generates a fake JWT-like token for AuthContext
  login: async (email, password) => {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (email && password) {
          // Creating a fake payload that matches our AuthContext structure
          const payload = {
            email: email,
            roles: ['Student'], // Defaulting to Student for mock
            exp: Math.floor(Date.now() / 1000) + (60 * 60 * 24)
          };
          
          // Mimicking a JWT token (simplified)
          const mockToken = `header.${btoa(JSON.stringify(payload))}.signature`;
          
          resolve({
            token: mockToken,
            user: { email, roles: ['Student'] }
          });
        } else {
          reject(new Error('Invalid credentials'));
        }
      }, 1000);
    });
  },

  // Mock registration
  registerStudent: async (data) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        console.log('Student registered:', data);
        resolve({ message: 'Student registered successfully' });
      }, 1000);
    });
  },

  registerInstructor: async (data) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        console.log('Instructor registered:', data);
        resolve({ message: 'Instructor registered successfully' });
      }, 1000);
    });
  },

  registerCoordinator: async (data) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        console.log('Coordinator registered:', data);
        resolve({ message: 'Coordinator registered successfully' });
      }, 1000);
    });
  },
};
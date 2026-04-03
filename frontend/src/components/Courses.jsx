import React, { useState, useEffect } from 'react';
import { Star, Clock, Users, BookOpen } from 'lucide-react';
import { api } from '../services/Api';
import { ImageWithFallback } from './figma/ImageWithFallback';

export const courseImages = [
  'https://images.unsplash.com/photo-1583508915901-b5f84c1dcde1?w=400',
  'https://images.unsplash.com/photo-1675557009285-b55f562641b9?w=400',
  'https://images.unsplash.com/photo-1547621008-d6d6d2e28e81?w=400',
  'https://images.unsplash.com/photo-1618788372246-79faff0c3742?w=400',
  'https://images.unsplash.com/photo-1660616246653-e2c57d1077b9?w=400',
  'https://images.unsplash.com/photo-1744868562210-fffb7fa882d9?w=400',
];

export const Courses = () => {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const data = await api.getCourses();
        // Add images to courses
        const coursesWithImages = data.map((course, index) => ({
          ...course,
          thumbnail: courseImages[index % courseImages.length],
        }));
        setCourses(coursesWithImages);
      } catch (error) {
        console.error('Error fetching courses:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  const getLevelColor = (level) => {
    const colors = {
      Beginner: 'bg-green-100 text-green-700',
      Intermediate: 'bg-yellow-100 text-yellow-700',
      Advanced: 'bg-red-100 text-red-700',
    };
    return colors[level] || 'bg-gray-100 text-gray-700';
  };

  if (loading) {
    return (
      <section id="courses" className="py-20 bg-gradient-to-br from-indigo-50 via-white to-violet-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <div className="inline-block animate-spin rounded-full h-12 w-12 border-4 border-teal-200 border-t-teal-600"></div>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section id="courses" className="py-20 bg-gradient-to-br from-indigo-50 via-white to-violet-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="text-center mb-16">
          <h2 className="text-4xl md:text-5xl font-bold mb-4 bg-gradient-to-r from-teal-600 via-cyan-600 to-violet-600 bg-clip-text text-transparent">
            Popular Courses
          </h2>
          <p className="text-lg text-gray-600 max-w-3xl mx-auto">
            Learn from industry experts and gain practical skills with our highly-rated courses
          </p>
        </div>

        {/* Courses Grid */}
        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
          {courses.map((course) => (
            <div
              key={course.id}
              className="bg-white rounded-2xl shadow-lg hover:shadow-2xl transition-all duration-300 overflow-hidden group hover:scale-105"
            >
              {/* Course Image */}
              <div className="relative h-48 overflow-hidden">
                <ImageWithFallback
                  src={course.thumbnail}
                  alt={course.name}
                  className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
                />
                <div className="absolute top-4 right-4">
                  <span className={`px-3 py-1 rounded-full text-sm font-semibold ${getLevelColor(course.level)}`}>
                    {course.level}
                  </span>
                </div>
              </div>

              {/* Course Content */}
              <div className="p-6">
                <div className="mb-3">
                  <span className="inline-block px-3 py-1 bg-violet-100 text-violet-700 text-sm rounded-full">
                    {course.category}
                  </span>
                </div>

                <h3 className="text-xl font-bold text-gray-800 mb-2 group-hover:text-violet-600 transition-colors">
                  {course.name}
                </h3>

                <p className="text-gray-600 text-sm mb-4">{course.instructor}</p>

                {/* Rating and Stats */}
                <div className="flex items-center gap-4 mb-4 text-sm text-gray-600">
                  <div className="flex items-center gap-1">
                    <Star className="w-4 h-4 fill-yellow-400 text-yellow-400" />
                    <span className="font-semibold text-gray-800">{course.rating}</span>
                  </div>
                  <div className="flex items-center gap-1">
                    <Users className="w-4 h-4" />
                    <span>{course.students?.toLocaleString() || 0}</span>
                  </div>
                  <div className="flex items-center gap-1">
                    <Clock className="w-4 h-4" />
                    <span>{course.duration}</span>
                  </div>
                </div>

                {/* Enroll Button */}
                <button className="w-full py-3 bg-gradient-to-r from-teal-500 to-cyan-500 text-white rounded-lg hover:shadow-lg transition-all transform hover:scale-105 flex items-center justify-center gap-2">
                  <BookOpen className="w-5 h-5" />
                  Enroll Now
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};
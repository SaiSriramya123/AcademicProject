ALTER TABLE Performances
ADD CONSTRAINT FK_Performances_Student_StudentId
FOREIGN KEY (StudentId) REFERENCES Student(StudentId)
ON DELETE NO ACTION;

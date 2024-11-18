export interface StudyGroupDTO {
  id: string;
  studyGroupName: string;
  idExamSubject: string;
  status?: boolean | null;
  isDeleted: boolean;
}

# by Falk Meyer, 2021-03-21

# imports
# =======================
# system
import unittest
import datetime
# local
from analysisUtils.AnalysisPlotter import AnalysisPlotter

# logic
# ======================

class AnalysisPlotterTests(unittest.TestCase):
    """
    Class for AnalysisPlotter unit tests
    """

    def test_construction(self):
        # input
        title = 'title'
        author = 'author'
        subject = 'subject'
        keywords = 'keyword1 keyword2'

        # logic
        plotter = AnalysisPlotter(title, author, subject, keywords)

        # tests
        self.assertEqual(plotter.mdTitle, title)
        self.assertEqual(plotter.mdAuthor, author)
        self.assertEqual(plotter.mdSubject, subject)
        self.assertEqual(plotter.mdKeywords, keywords)
        self.assertEqual(plotter.tableRowsPerPage, 50)

    def test_set_metadata(self):
        # input
        title = 'title'
        author = 'author'
        subject = 'subject'
        keywords = 'keyword1 keyword2'

        # mock
        infoD = {}
        pdf = type('', (), {})()
        pdf.infodict = lambda: infoD

        # logic
        plotter = AnalysisPlotter()
        plotter.mdTitle = title
        plotter.mdAuthor = author
        plotter.mdSubject = subject
        plotter.mdKeywords = keywords
        plotter.set_metadata(pdf)

        # tests
        self.assertEqual(infoD['Title'], title)
        self.assertEqual(infoD['Author'], author)
        self.assertEqual(infoD['Subject'], subject)
        self.assertEqual(infoD['Keywords'], keywords)
        self.assertEqual(infoD['CreationDate'], datetime.datetime.today())

if __name__ == '__main__':
    unittest.main()

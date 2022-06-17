# by Falk Meyer, 2021-03-20

# imports
# =======================
# system
import datetime
# third-party
import pandas.plotting as pndPlot
import matplotlib.pyplot as plt
from matplotlib.backends.backend_pdf import PdfPages

# logic
# ======================

class AnalysisPlotter:
    """
    Class for plotting an analysis
    """

    def __init__(self,
                 title='Analysis report: Buildings in Rotterdam positively affected by highway tunnel construction',
                 author='Falk Meyer',
                 subject='Buildings positively affected of highway tunnel construction in Rotterdam',
                 keywords='Buildings Rotterdam GIS Engineer'):
        """
        Initialize the pdf file with the given metadata
        :param title: Title of the file
        :param author: Author of the file
        :param subject: Subject of the file
        :param keywords: Keywords related to the file
        """
        self.mdTitle = title
        self.mdAuthor = author
        self.mdSubject = subject
        self.mdKeywords = keywords
        self.tableRowsPerPage = 50

    def set_metadata(self, pdf):
        """
        Set the file's metadata
        :param pdf: pdf object
        :return:
        """
        d = pdf.infodict()
        d['Title'] = self.mdTitle
        d['Author'] = self.mdAuthor
        d['Subject'] = self.mdSubject
        d['Keywords'] = self.mdKeywords
        d['CreationDate'] = datetime.datetime.today()

    def plot_map(self, pdf, subjects, bufferArea, bufferOrigin, figureSize):
        """
        Plot the map in a diagram
        :param pdf: The pdf object
        :param subjects: The subjects of the analysis
        :param bufferArea: The analysed area
        :param bufferOrigin: The buffer origin object
        :param figureSize: The figure size
        :return:
        """
        # plot map
        fig, ax = plt.subplots(figsize=figureSize)
        ax.set_aspect('equal')

        # set title and label
        plt.title(self.mdTitle, pad=40)
        plt.xlabel('longitude')
        plt.ylabel('latitude')

        # building page
        subjects.plot(ax=ax, color='blue', figsize=figureSize)
        bufferArea.plot(ax=ax, color='green', alpha=0.2)
        bufferOrigin.plot(ax=ax, color='black')
        pdf.savefig()
        plt.close()

    def plot_table_page(self, pdf, plotFrame, figureSize, pageNumber):
        """
        Plot one table page
        :param pdf: The pdf object
        :param plotFrame: The data to be plotted
        :param figureSize: The figure size
        :param pageNumber: The page number
        :return:
        """
        fig, ax = plt.subplots(figsize=figureSize)
        plt.title('Analysis subjects information #' + str(pageNumber))
        ax.axis('tight')
        ax.axis('off')
        pndPlot.table(ax=ax, data=plotFrame, loc='upper left')

        pdf.savefig()
        plt.close()

    def plot_table(self, pdf, subjects, figureSize):
        """
        Plot an attribute table of the input subjects
        :param pdf: The pdf object
        :param subjects: The subjects of the analysis
        :param figureSize: The figure size
        :return:
        """

        # table pages
        plotFrame = subjects.drop(['geometry'], axis=1)
        tableLength = len(subjects.index)
        rowPlotted = self.tableRowsPerPage
        pageNumber = 1

        # print pages
        while rowPlotted < tableLength:
            self.plot_table_page(pdf, plotFrame[rowPlotted - self.tableRowsPerPage:rowPlotted], figureSize, pageNumber)
            rowPlotted += self.tableRowsPerPage
            pageNumber += 1

        # final page
        if rowPlotted - tableLength > 0:
            self.plot_table_page(pdf, plotFrame[rowPlotted - self.tableRowsPerPage:tableLength], figureSize, pageNumber)

    def plot(self, subjects, bufferArea, bufferOrigin, fileName='AnalysisReport.pdf', format='A4_Portrait', table=True):
        """
        Plot the analysis document
        :param subjects: The subjects of the analysis
        :param bufferArea: The analysed area
        :param bufferOrigin: The buffer origin object
        :param fileName: Desired file name of the pdf file to be printed
        :param format: File format i.e. size
        :param table: Indicates if a table should be printed as well
        :return:
        """
        # set plot properties
        if format == 'A4_Portrait':
            figureSize = (8.27, 11.69)
            self.tableRowsPerPage = 50
        elif format == 'A4_Landscape':
            figureSize = (11.69, 8.27)
            self.tableRowsPerPage = 35
        else:
            figureSize = (8.27, 11.69)
            self.tableRowsPerPage = 50

        with PdfPages(fileName) as pdf:

            # set metadata
            self.set_metadata(pdf)

            # plot the map
            self.plot_map(pdf, subjects, bufferArea, bufferOrigin, figureSize)

            # plot table if requested
            if table:
                self.plot_table(pdf, subjects, figureSize)
